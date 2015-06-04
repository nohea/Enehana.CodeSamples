using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPatchWithServiceStack.Operations
{
    // https://tools.ietf.org/html/draft-ietf-appsawg-json-patch-10
    // http://stackoverflow.com/questions/22820318/http-patch-handling-arrays-deletion-and-nested-key-creation
    // http://williamdurand.fr/2014/02/14/please-do-not-patch-like-an-idiot/
    /// <summary>
    /// JSON Patch , RFC 6902. Used for partial updates. 
    /// </summary>
    public class JsonPatchElement
    {
        public string op { get; set; } // "add", "remove", "replace", "move", "copy" or "test"
        public string path { get; set; }
        public string value { get; set; }
    }
}