/*
   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.

   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.opensource.org/licenses/ms-rl

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;

namespace CodeOwls.StudioShell.Cmdlets
{
    public class PSObjectBindingList : BindingList<PSObject>, ITypedList
    {
        private readonly IEnumerable<string> _propertyNames;
        private readonly IEnumerable<string> _excludePropertyNames;
        private bool _useNativeTypes;

        public PSObjectBindingList()
        {
             
        }

        public PSObjectBindingList(IEnumerable<string> propertyNames)
        {
            _propertyNames = propertyNames;
        }

        public PSObjectBindingList(IEnumerable<string> propertyNames, IEnumerable<string> excludePropertyNames)
        {
            _propertyNames = propertyNames;
            _excludePropertyNames = excludePropertyNames;
        }

        public PSObjectBindingList(IEnumerable<string> propertyNames, bool useNativeTypes)
        {
            _propertyNames = propertyNames;
            _useNativeTypes = useNativeTypes;
        }

        #region Implementation of ITypedList
        protected override void InsertItem(int index, PSObject item)
        {
            
            PSObject pso = new PSObject();
            item.Properties.Where(
                p=>IsIncludableProperty(p) && ! IsExcludableProperty(p)
            ).ToList().ForEach( 
                p=>
                    {
                        object value = p.Value;
                        var psov = p.Value as PSObject;
                        if (null != psov)
                        {
                            value = psov.BaseObject;
                        }
                        pso.Properties.Add(new PSNoteProperty(p.Name, value));
                    }
                );

            base.InsertItem(index, pso);
        }

        private bool IsIncludableProperty(PSPropertyInfo psPropertyInfo)
        {
            if( null == _propertyNames || ! _propertyNames.Any())
            {
                return true;
            }
            var props = from s in _propertyNames
            where (
                IsWildcardMatch( s, psPropertyInfo ) ||
                StringComparer.InvariantCultureIgnoreCase.Equals( s, psPropertyInfo.Name )
            )
            select s;

            return props.Any();
        }
        private bool IsExcludableProperty(PSPropertyInfo psPropertyInfo)
        {
            if( null == _excludePropertyNames || !_excludePropertyNames.Any() )
            {
                return false;
            }
            var props = from s in _excludePropertyNames
                        where(
                            IsWildcardMatch(s, psPropertyInfo) ||
                            StringComparer.InvariantCultureIgnoreCase.Equals(s, psPropertyInfo.Name)
                        )
                        select s;

            return props.Any();
        }
        private bool IsWildcardMatch(string s, PSPropertyInfo psPropertyInfo)
        {
            if (!WildcardPattern.ContainsWildcardCharacters(s))
            {
                return false;
            }

            var w = new WildcardPattern(s,WildcardOptions.IgnoreCase);
            return w.IsMatch(psPropertyInfo.Name);
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return "PSObject";
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            PSObject o = this[0];
            var items = (from p in o.Properties
                         select new PSPropertyInfoDescriptor(p, _useNativeTypes)).ToArray();
            var c = new PropertyDescriptorCollection(items);
            return c;
        }

        #endregion
    }

    internal class PSPropertyInfoDescriptor : PropertyDescriptor
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();
        private readonly PSPropertyInfo _psPropertyInfo;
        private readonly bool _useNativeTypes;
        private object _value;
        private Type _type;

        public PSPropertyInfoDescriptor(PSPropertyInfo psPropertyInfo, bool useNativeTypes) : 
            base( psPropertyInfo.Name, null )
        {
            _psPropertyInfo = psPropertyInfo;
            _useNativeTypes = useNativeTypes;
            LoadAttributesFromPSPropertyInfo();
        }

        private void LoadAttributesFromPSPropertyInfo()
        {
            List<Attribute> attrs = new List<Attribute>( AttributeArray ?? new Attribute[0] );
            attrs.Add( new DisplayNameAttribute( _psPropertyInfo.Name ));
            attrs.Add( new SerializableAttribute());
            attrs.Add(BrowsableAttribute.Yes);
            AttributeArray = attrs.ToArray();
        }

        #region Overrides of PropertyDescriptor
        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            return base.GetChildProperties(instance, filter);
        }
        public override TypeConverter Converter
        {
            get
            {
                return base.Converter;
            }
        }
        public override AttributeCollection Attributes
        {
            get
            {
                return base.Attributes;
            }
        }
        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            PSObject pso = component as PSObject;
            if (null == pso)
            {
                return null;
            }

            if (null == _psPropertyInfo)
            {
                return null;
            }

            var prop = pso.Properties[_psPropertyInfo.Name];
            if (null == prop)
            {
                return null;
            }

            object value = prop.Value;
            return value;
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get { return  typeof( PSObject ); }
        }

        public override bool IsReadOnly
        {
            get { return ! _psPropertyInfo.IsSettable;  }
        }

        public override Type PropertyType
        {
            get
            {
                if( null != _type)
                {
                    return _type;
                }
                if( String.IsNullOrEmpty( _psPropertyInfo.TypeNameOfValue))
                {
                    return typeof (string);
                }
                if( _cache.ContainsKey( _psPropertyInfo.TypeNameOfValue))
                {
                    _type = _cache[_psPropertyInfo.TypeNameOfValue];
                    return _type;
                }
                _type = Type.GetType(_psPropertyInfo.TypeNameOfValue);
                if( null != _type )
                {
                    return _type;
                }

                var asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach( var asm in asms )
                {
                    try
                    {
                        var foundType = (from type in asm.GetExportedTypes()
                                         where type.FullName == _psPropertyInfo.TypeNameOfValue
                                         select type).FirstOrDefault();
                        if( null != foundType )
                        {
                            _type = foundType;
                            break;
                        }
                    }
                    catch 
                    {
                    }
                }
                
                if( null == _type )
                {
                    _type = typeof (string);
                }

                _cache[_psPropertyInfo.TypeNameOfValue] = _type;

                return _type;
            }
        }

        #endregion
    }
}
