/* 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

 * 
 * PokeIn Comet Library (pokein.codeplex.com)
 * Copyright © 2010 Oguz Bastemur http://pokein.codeplex.com (info@pokein.com)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace PokeIn
{
    internal class Definition
    {
        public List<string> DefinedClasses;
        public Dictionary<string, object> ClassObjects;
        public Dictionary<string, SubMember> ClassMembers;
        private string _json;

        public string JSON
        {
            get { return _json; }
        }

        public Definition()
        {
            DefinedClasses = new List<string>();
            ClassObjects = new Dictionary<string, object>();
            ClassMembers = new Dictionary<string, SubMember>();
            _json = "";
        }

        public void Add(string className, ref object definedObject, string instanceName)
        {
            if (DefinedClasses.Contains(className))
            {
                if (!ClassObjects.ContainsKey(instanceName + "." + className))
                {
                    ClassObjects.Add(instanceName + "." + className, definedObject);
                }
                return;
            }

            DefinedClasses.Add(className);

            Type t = definedObject.GetType();

            ClassObjects[instanceName + "." + className] = definedObject;
            System.Reflection.MethodInfo[] methods = t.GetMethods();

            bool enablePokeinSafety = false;
            
            System.Reflection.FieldInfo fi = t.GetField("PokeInSafe") ;
            if (fi != null)
            {
                enablePokeinSafety = Convert.ToBoolean( fi.GetValue(definedObject) );
            }

            StringBuilder sbJson = new StringBuilder();

            sbJson.Append("function ");
            sbJson.Append(className);
            sbJson.Append("(){}"); 

            for (int i = 0, ml = methods.Length; i < ml; i++)
            {
                if (methods[i].IsPrivate)
                    continue;

                if (methods[i].ReturnParameter.ParameterType != typeof(void))
                    continue;

                if (enablePokeinSafety)
                {
                    if (!methods[i].Name.StartsWith("__"))
                        continue;
                }

                System.Reflection.ParameterInfo[] paramz = methods[i].GetParameters();
                bool isCompatible = true;
                foreach (System.Reflection.ParameterInfo param in paramz)
                {
                    if (!param.ParameterType.IsSerializable)
                    {
                        isCompatible = false;
                        break;
                    }
                    if (param.ParameterType == typeof(EventArgs) )
                    {
                        isCompatible = false;
                        break;
                    }
                }
                if (!isCompatible)
                    continue;

                SubMember sm = new SubMember();
                string completeName = className + "." + methods[i].Name;

                sbJson.Append(completeName);
                sbJson.Append("=function(");

                bool isFirst = true;
                
                List<string> stringList = new List<string>();
                List<string> letterList = new List<string>();

                int indexer = 0;
                StringBuilder letterz = new StringBuilder();

                foreach (System.Reflection.ParameterInfo param in paramz)
                {
                    if(!isFirst)
                    {
                        letterz.Append(",");
                    }
                    
                    sm.ParameterTypes.Add(param.ParameterType);
                    string paramName = "a"+(indexer).ToString();
                    letterz.Append(paramName);

                    if (param.ParameterType == typeof(String))
                    {
                        stringList.Add(paramName + "=PokeIn.StrFix(" + paramName + ");");
                    }
                    letterList.Add(paramName);

                    isFirst = false;
                    indexer++;
                }
                sbJson.Append(letterz.ToString(0,letterz.Length));
                sbJson.Append("){");

                foreach (string str in stringList)
                    sbJson.Append(str);

                sbJson.Append("PokeIn.Send(PokeIn.GetClientId() + \".");
                sbJson.Append(completeName + "(");

                isFirst = true;
                foreach (string strLetter in letterList)
                {
                    if(!isFirst)
                    {
                        sbJson.Append( "+\"," );
                    }
                    sbJson.Append( "\"+" + strLetter );
                    isFirst = false;
                }
                if (!isFirst)
                {
                    sbJson.Append( "+\"" );
                }
                sbJson.Append( ");\");}\n" );

                lock (_json)
                {
                    _json += sbJson.ToString(0,sbJson.Length);
                }
                sm.SetMethod(methods[i]);
                if(!ClassMembers.ContainsKey(completeName))
                        ClassMembers.Add(completeName, sm);
            } 
        }
    };

}
