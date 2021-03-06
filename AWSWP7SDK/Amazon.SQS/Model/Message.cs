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

using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Amazon.SQS.Model
{
    /// <summary>
    /// An element containing the information about the message.
    /// </summary>
    [XmlRootAttribute(Namespace = "http://queue.amazonaws.com/doc/2009-02-01/", IsNullable = false)]
    public class Message : SQSBaseResponse
    {
        private string messageIdField;
        private string receiptHandleField;
        private string MD5OfBodyField;
        private string bodyField;
        private List<Attribute> attributes;

        /// <summary>
        /// Gets and sets the MessageId property.
        /// Unique ID associated with the message.
        /// </summary>
        [XmlElementAttribute(ElementName = "MessageId")]
        public string MessageId
        {
            get { return this.messageIdField; }
            set { this.messageIdField = value; }
        }

        /// <summary>
        /// Sets the MessageId property
        /// </summary>
        /// <param name="messageId">Unique ID associated with the message.</param>
        /// <returns>this instance</returns>
        public Message WithMessageId(string messageId)
        {
            this.messageIdField = messageId;
            return this;
        }

        /// <summary>
        /// Checks if MessageId property is set
        /// </summary>
        /// <returns>true if MessageId property is set</returns>
        public bool IsSetMessageId()
        {
            return this.messageIdField != null;
        }

        /// <summary>
        /// Gets and sets the ReceiptHandle property.
        /// A string associated with a specific instance of receiving the message.
        /// </summary>
        [XmlElementAttribute(ElementName = "ReceiptHandle")]
        public string ReceiptHandle
        {
            get { return this.receiptHandleField; }
            set { this.receiptHandleField = value; }
        }

        /// <summary>
        /// Sets the ReceiptHandle property
        /// </summary>
        /// <param name="receiptHandle">A string associated with a specific instance of receiving the message.</param>
        /// <returns>this instance</returns>
        public Message WithReceiptHandle(string receiptHandle)
        {
            this.receiptHandleField = receiptHandle;
            return this;
        }

        /// <summary>
        /// Checks if ReceiptHandle property is set
        /// </summary>
        /// <returns>true if ReceiptHandle property is set</returns>
        public bool IsSetReceiptHandle()
        {
            return this.receiptHandleField != null;
        }

        /// <summary>
        /// Gets and sets the MD5OfBody property.
        /// An MD5 digest of the non-URL-encoded message body string.
        /// </summary>
        [XmlElementAttribute(ElementName = "MD5OfBody")]
        public string MD5OfBody
        {
            get { return this.MD5OfBodyField; }
            set { this.MD5OfBodyField = value; }
        }

        /// <summary>
        /// Sets the MD5OfBody property
        /// </summary>
        /// <param name="md5OfBody">An MD5 digest of the non-URL-encoded message body string.</param>
        /// <returns>this instance</returns>
        public Message WithMD5OfBody(string md5OfBody)
        {
            this.MD5OfBodyField = md5OfBody;
            return this;
        }

        /// <summary>
        /// Checks if MD5OfBody property is set
        /// </summary>
        /// <returns>true if MD5OfBody property is set</returns>
        public bool IsSetMD5OfBody()
        {
            return this.MD5OfBodyField != null;
        }

        /// <summary>
        /// Gets and sets the Body property.
        /// The message's contents (not URL encoded).
        /// </summary>
        [XmlElementAttribute(ElementName = "Body")]
        public string Body
        {
            get { return this.bodyField; }
            set { this.bodyField = value; }
        }

        /// <summary>
        /// Sets the Body property
        /// </summary>
        /// <param name="body">The message's contents (not URL encoded).</param>
        /// <returns>this instance</returns>
        public Message WithBody(string body)
        {
            this.bodyField = body;
            return this;
        }

        /// <summary>
        /// Checks if Body property is set
        /// </summary>
        /// <returns>true if Body property is set</returns>
        public bool IsSetBody()
        {
            return this.bodyField != null;
        }

        /// <summary>
        /// Gets the Attribute property.
        /// Name and value pair of an attribute associated with the queue.
        /// </summary>
        [XmlElementAttribute(ElementName = "Attribute")]
        public List<Attribute> Attribute
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new List<Attribute>();
                }
                return this.attributes;
            }
        }

        /// <summary>
        /// Sets the Attribute property
        /// </summary>
        /// <param name="list">Name and value pair of an attribute associated with the queue.</param>
        /// <returns>this instance</returns>
        public Message WithAttribute(params Attribute[] list)
        {
            foreach (Attribute item in list)
            {
                Attribute.Add(item);
            }
            return this;
        }

        /// <summary>
        /// Checks if Attribute property is set
        /// </summary>
        /// <returns>true if Attribute property is set</returns>
        public bool IsSetAttribute()
        {
            return (Attribute.Count > 0);
        }
    }
}
