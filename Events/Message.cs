//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace L6.CommandProcessor.DomainEvents
//{
//    public class Message : EventMessage
//    {

//        new public Headers 
//    }

//    [DataContract, Serializable]
//    public class EventMessage
//    {
//        /// <summary>
//        /// Initializes a new instance of the EventMessage class.
//        /// </summary>
//        public EventMessage()
//        {
//            this.Headers = new Dictionary<string, object>();
//        }

//        /// <summary>
//        /// Gets the metadata which provides additional, unstructured information about this message.
//        /// </summary>
//        [DataMember]
//        public Dictionary<string, object> Headers { get; private set; }

//        /// <summary>
//        /// Gets or sets the actual event message body.
//        /// </summary>
//        [DataMember]
//        public object Body { get; set; }
//    }
//}
