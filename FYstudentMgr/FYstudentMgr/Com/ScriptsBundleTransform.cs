using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace FYstudentMgr.Common
{
   public class ScriptsBundleTransform : IBundleTransform    
   {   
       public string ScriptsPath { get; set; }        
       public string Version { get; set; }        
       public string Minified { get; set; }        
       public string Full { get; set; }        
       public ScriptsBundleTransform(){        }        
       public ScriptsBundleTransform(string path, string version, string minified, string full)        
       {            
           ScriptsPath = path;            
           Version = version;            
           Minified = minified;            
           Full = full;        
       }        
       public void Process(BundleContext context, BundleResponse response)        
       {            
           string scriptsRoot = context.HttpContext.Server.MapPath(ScriptsPath);            
           if (!Directory.Exists(scriptsRoot))                
               Directory.CreateDirectory(scriptsRoot);            
           //  if minified file name specified...            
           if (!string.IsNullOrEmpty(Minified))            
           {                
               using (TextWriter writer = File.CreateText(Path.Combine(scriptsRoot, Minified)))                
               {                    
                   writer.Write(response.Content);                
               }            
           }           
           //  if full file name specified...            
           if (!string.IsNullOrEmpty(Full))            
           {                
               using (Stream writer = File.OpenWrite(Path.Combine(scriptsRoot, Full)))                
               {                    
                   foreach (var file in response.Files)                    
                   {                        
                       file.VirtualFile.Open().CopyTo(writer);                    
                   }                
               }            
           }        
       }    
   }
}