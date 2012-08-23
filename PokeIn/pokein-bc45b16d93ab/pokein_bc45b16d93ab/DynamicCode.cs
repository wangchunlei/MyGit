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
using System.Text.RegularExpressions;

namespace PokeIn
{
    internal class DynamicCode
    {
        public DynamicCode()
        {
            Definitions = new Definition();
        }
        
        public static Definition Definitions;
 
        private object[] ParseFunctionParams(string methodClass, string param)
        { 
            List<object> parameterList = new List<object>();

            string parameterError = "";
            if (param.Length > 0)
            {
                string[] paramList = param.Split(',');

                SubMember func;
                Definitions.ClassMembers.TryGetValue(methodClass, out func);

                int pos = 0;
                while (pos < paramList.Length)
                {
                    string subP = paramList[pos].Trim();
                    if (subP.StartsWith("\""))
                    {
                        if (!subP.EndsWith("\""))
                        {
                            subP = paramList[pos];
                            while (pos + 1 < paramList.Length && !subP.EndsWith("\""))
                            {
                                subP += "," + paramList[++pos];
                            }
                            subP = subP.Trim();
                        }
                        if (subP.Length > 2)
                        {
                            parameterList.Add(subP.Substring(1, subP.Length - 2));
                        }
                        else
                        {
                            parameterList.Add("");
                        }
                        pos++;
                        continue;
                    }
                    if (subP == "'")
                    {
                        pos += 2;
                        parameterList.Add(',');
                        continue;
                    }
                    if (subP.StartsWith("'") && subP.EndsWith("'"))
                    {
                        if (subP.Length > 2)
                        {
                            parameterList.Add(subP.ToCharArray()[1]);
                        }
                        else
                        {
                            parameterList.Add("");
                        }
                        pos++;
                        continue;
                    } 

                    if (func.ParameterTypes.Count > parameterList.Count)
                    {
                        try
                        {
                            parameterList.Add(Convert.ChangeType(subP, func.ParameterTypes[parameterList.Count]));
                        }
                        catch (Exception e)
                        {
                            parameterError += e.Message + " | ";
                        }
                        pos++;
                        continue;
                    }

                    parameterList.Add(subP);
                    pos++;
                }
            }
            if (parameterError.Length > 0)
            {
                throw new Exception(parameterError);
            }
            return parameterList.ToArray();   
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        public bool Run(string callString)
        {
            string[] safeMethods = callString.Split('\r');

            foreach (string stringToCall in safeMethods)
            {
                _errorMessage = "";
                Regex methods = new Regex(@"(?<Client>[a-zA-Z]{1}[a-zA-Z0-9]{0,})(?<dot1>[\.]{1})(?<Class>[a-zA-Z]{1}[a-zA-Z0-9]{0,})(?<dot2>[\.]{1})(?<Function>[a-zA-Z]{1}[a-zA-Z0-9]{0,})(?<lp>[(]{1})(?<Params>.{0,})(?<rp>[)]{1}[;]?)");
                MatchCollection mcMethods = methods.Matches(stringToCall);

                for (int i = 0; i < mcMethods.Count; i++)
                {
                    bool status = mcMethods[i].Success;
                    if (!status)
                        continue;

                    string clientName = mcMethods[i].Groups["Client"].Value.Trim();
                    string className = mcMethods[i].Groups["Class"].Value.Trim();
                    string methodName = mcMethods[i].Groups["Function"].Value.Trim();
                    string param = mcMethods[i].Groups["Params"].Value.Trim();

                    object[] paramList = ParseFunctionParams(className + "." + methodName, param);

                    object definedClass;
                    SubMember func = null;

                    Definitions.ClassObjects.TryGetValue(clientName + "." + className, out definedClass);

                    if (definedClass != null)
                    {
                        Definitions.ClassMembers.TryGetValue(className + "." + methodName, out func);
                    }

                    try
                    {
                        func.MethodInfo.Invoke(definedClass, paramList);
                    }
                    catch (Exception e)
                    {
                        _errorMessage = e.Message;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}


