/*
   Copyright 2011 Microsoft Corp.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

*/

/*******************************************************************************
 *  AWS SDK for WP7
 */

using System;
using System.Globalization;
using System.IO;
using Amazon.S3.Util;
using Amazon.Util;

namespace Amazon.S3.Model
{
    /// <summary>
    /// The GetObjectResponse contains any header or metadata returned by S3. 
    /// GetObjectResponse's contain resources that need to be disposed. The
    /// recommended way for handling GetObjectResponse objects is wrapping them
    /// in using clauses, like so:
    /// <code>
    /// using (GetObjectResponse response = s3Client.GetObject(request))
    /// {
    ///     ...
    /// }
    /// </code>
    /// This will ensure that any network resources, file streams and web headers
    /// have been returned back to the system for future use.
    /// </summary>
    public class GetObjectResponse : S3Response
    {
        private string etag;
        private long contentLength;
        private string contentType;
        private string versionId;
        string bucketName;
        string key;

        /// <summary>
        /// Gets and sets the BucketName property.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Gets and sets the Key property.
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets and sets the ETag property.
        /// </summary>
        public string ETag
        {
            get { return this.etag; }
            set { this.etag = value; }
        }

        /// <summary>
        /// Gets and sets the ContentType property.
        /// </summary>
        public string ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        /// <summary>
        /// Gets and sets the ContentLength property.
        /// </summary>
        public long ContentLength
        {
            get { return this.contentLength; }
            set { this.contentLength = value; }
        }

        /// <summary>
        /// Gets and sets the VersionId property.
        /// This is the version-id of the S3 object
        /// </summary>
        public string VersionId
        {
            get { return this.versionId; }
            set { this.versionId = value; }
        }

        /// <summary>
        /// Gets and sets the Headers property.
        /// </summary>
        public override System.Net.WebHeaderCollection Headers
        {
            set
            {
                base.Headers = value;

                string hdr = null;
                if (!String.IsNullOrEmpty(hdr = value[AWSSDKUtils.ETagHeader]))
                {
                    this.ETag = hdr;
                }

                if (!String.IsNullOrEmpty(hdr = value[AWSSDKUtils.ContentTypeHeader]))
                {
                    this.ContentType = hdr;
                }

                if (!String.IsNullOrEmpty(hdr = value[AWSSDKUtils.ContentLengthHeader]))
                {
                    this.ContentLength = System.Convert.ToInt64(hdr, CultureInfo.InvariantCulture);
                }

                if (!String.IsNullOrEmpty(hdr = value[S3Constants.AmzVersionIdHeader]))
                {
                    this.VersionId = hdr;
                }
            }
        }

        /// <summary>
        /// Writes the content of the ResponseStream a file indicated by the filePath argument.
        /// </summary>
        /// <param name="filePath">The location where to write the ResponseStream</param>
        public void WriteResponseStreamToFile(string filePath)
        {
            // Make sure the directory exists to write too.
            FileInfo fi = new FileInfo(filePath);
            Directory.CreateDirectory(fi.DirectoryName);

            //Stream downloadStream = new BufferedStream(new FileStream(filePath, FileMode.Create));
            Stream downloadStream = new FileStream(filePath, FileMode.Create);
            try
            {
                long current = 0;
                //BufferedStream bufferedStream = new BufferedStream(this.ResponseStream);
                Stream bufferedStream = this.ResponseStream;
                byte[] buffer = new byte[S3Constants.DefaultBufferSize];
                int bytesRead = 0;
                while ((bytesRead = bufferedStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    downloadStream.Write(buffer, 0, bytesRead);
                    current += bytesRead;

                    this.OnRaiseProgressEvent(bytesRead, current, this.contentLength);
                }
            }
            finally
            {
                downloadStream.Close();
            }
        }

        #region Progress Event

        /// <summary>
        /// The event for Write Object progress notifications. All
        /// subscribers will be notified when a new progress
        /// event is raised.
        /// </summary>
        /// <remarks>
        /// Subscribe to this event if you want to receive
        /// put object progress notifications. Here is how:<br />
        /// 1. Define a method with a signature similar to this one:
        /// <code>
        /// private void displayProgress(object sender, WriteObjectProgressEventArgs args)
        /// {
        ///     Console.WriteLine(args);
        /// }
        /// </code>
        /// 2. Add this method to the Put Object Progress Event delegate's invocation list
        /// <code>
        /// GetObjectResponse response = s3Client.GetObject(request);
        /// response.WriteObjectProgressEvent += displayProgress;
        /// </code>
        /// </remarks>
        public event EventHandler<WriteObjectProgressEventArgs> WriteObjectProgressEvent;

        /// <summary>
        /// The "handler" will be notified every time a put
        /// object progress event is raised.
        /// </summary>
        /// <param name="handler">A method that consumes the put object progress notification</param>
        /// <returns>this instance of the PutObjectRequest</returns>
        public GetObjectResponse WithSubscriber(EventHandler<WriteObjectProgressEventArgs> handler)
        {
            this.WriteObjectProgressEvent += handler;
            return this;
        }

        #endregion

        /// <summary>
        /// This method is called by a producer of write object progress
        /// notifications. When called, all the subscribers in the 
        /// invocation list will be called sequentially.
        /// </summary>
        /// <param name="incrementTransferred">The number of bytes transferred since last event</param>
        /// <param name="transferred">The number of bytes transferred</param>
        /// <param name="total">The total number of bytes to be transferred</param>
        internal void OnRaiseProgressEvent(long incrementTransferred, long transferred, long total)
        {
            // Make a temporary copy of the event to avoid the possibility of
            // a race condition if the last and only subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<WriteObjectProgressEventArgs> handler = WriteObjectProgressEvent;
            try
            {
                // Event will be null if there are no subscribers
                if (handler != null)
                {
                    // This automatically calls all subscribers sequentially
                    // http://msdn.microsoft.com/en-us/library/ms173172%28VS.80%29.aspx
                    handler(this, new WriteObjectProgressEventArgs(this.bucketName, this.key, this.versionId, incrementTransferred, transferred, total));
                }
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Encapsulates the information needed to provide
    /// download progress for the Write Object Event.
    /// </summary>
    public class WriteObjectProgressEventArgs : TransferProgressArgs
    {
        string bucketName;
        string key;
        string versionId;

        /// <summary>
        /// The constructor takes the number of
        /// currently transferred bytes and the
        /// total number of bytes to be transferred
        /// </summary>
        /// <param name="bucketName">The bucket name for the S3 object being written.</param>
        /// <param name="key">The object key for the S3 object being written.</param>
        /// <param name="versionId">The version-id of the S3 object.</param>
        /// <param name="incrementTransferred">The number of bytes transferred since last event</param>
        /// <param name="transferred">The number of bytes transferred</param>
        /// <param name="total">The total number of bytes to be transferred</param>
        internal WriteObjectProgressEventArgs(string bucketName, string key, string versionId, long incrementTransferred, long transferred, long total)
            : base(incrementTransferred, transferred, total)
        {
            this.bucketName = bucketName;
            this.key = key;
            this.versionId = versionId;
        }

        /// <summary>
        /// Gets the bucket name for the S3 object being written.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
        }

        /// <summary>
        /// Gets the object key for the S3 object being written.
        /// </summary>
        public string Key
        {
            get { return this.key; }
        }

        /// <summary>
        /// Gets the version-id of the S3 object.
        /// </summary>
        public string VersionId
        {
            get { return this.versionId; }
        }
    }

}