using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace FYstudentMgr.Common
{
    public static class BundleExtensions {
        public static string Version = "1.0.0"; 
        public static string ScriptsPath = "Cdn"; 
        public static Bundle Production(this Bundle bundle, string cdn, string root, string minified, string full = "")
        {
            var transform = new ScriptsBundleTransform()
            { 
            Version = Version, 
            ScriptsPath = System.IO.Path.Combine("~/", ScriptsPath, root), 
            Minified = minified, 
            Full = full }; 
         bundle.Transforms.Add(transform); 
         bundle.CdnPath = cdn + "/" + root + "/" + string.Format("{0}?{1}", minified, Version); 
        return bundle; 
        } 
    }
}