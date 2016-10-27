namespace UnityTest
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public class MemberResolver
    {
        private object m_CallingObjectRef;
        private MemberInfo[] m_Callstack;
        private readonly GameObject m_GameObject;
        private readonly string m_Path;

        public MemberResolver(GameObject gameObject, string path)
        {
            path = path.Trim();
            this.ValidatePath(path);
            this.m_GameObject = gameObject;
            this.m_Path = path.Trim();
        }

        private object GetBaseObject()
        {
            if (!string.IsNullOrEmpty(this.m_Path))
            {
                char[] separator = new char[] { '.' };
                string type = this.m_Path.Split(separator)[0];
                Component component = this.m_GameObject.GetComponent(type);
                if (component != null)
                {
                    return component;
                }
            }
            return this.m_GameObject;
        }

        private MemberInfo[] GetCallstack()
        {
            if (this.m_Path == string.Empty)
            {
                return new MemberInfo[0];
            }
            char[] separator = new char[] { '.' };
            Queue<string> queue = new Queue<string>(this.m_Path.Split(separator));
            System.Type propertyType = this.GetBaseObject().GetType();
            if (propertyType != typeof(GameObject))
            {
                queue.Dequeue();
            }
            List<MemberInfo> list = new List<MemberInfo>();
            while (queue.Count != 0)
            {
                string fieldName = queue.Dequeue();
                FieldInfo field = GetField(propertyType, fieldName);
                if (field == null)
                {
                    PropertyInfo property = GetProperty(propertyType, fieldName);
                    if (property == null)
                    {
                        throw new InvalidPathException(fieldName);
                    }
                    propertyType = property.PropertyType;
                    MethodInfo getMethod = GetGetMethod(property);
                    list.Add(getMethod);
                }
                else
                {
                    propertyType = field.FieldType;
                    list.Add(field);
                    continue;
                }
            }
            return list.ToArray();
        }

        private static FieldInfo GetField(System.Type type, string fieldName)
        {
            return type.GetField(fieldName);
        }

        private static MethodInfo GetGetMethod(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetGetMethod();
        }

        public System.Type GetMemberType()
        {
            MemberInfo[] callstack = this.GetCallstack();
            if (callstack.Length == 0)
            {
                return this.GetBaseObject().GetType();
            }
            MemberInfo info = callstack[callstack.Length - 1];
            if (info is FieldInfo)
            {
                return (info as FieldInfo).FieldType;
            }
            if (info is MethodInfo)
            {
                return (info as MethodInfo).ReturnType;
            }
            return null;
        }

        private static PropertyInfo GetProperty(System.Type type, string propertyName)
        {
            return type.GetProperty(propertyName);
        }

        public object GetValue(bool useCache)
        {
            if (useCache && (this.m_CallingObjectRef != null))
            {
                object callingObjectRef = this.m_CallingObjectRef;
                for (int j = 0; j < this.m_Callstack.Length; j++)
                {
                    callingObjectRef = this.GetValueFromMember(callingObjectRef, this.m_Callstack[j]);
                }
                return callingObjectRef;
            }
            object baseObject = this.GetBaseObject();
            MemberInfo[] callstack = this.GetCallstack();
            this.m_CallingObjectRef = baseObject;
            List<MemberInfo> list = new List<MemberInfo>();
            for (int i = 0; i < callstack.Length; i++)
            {
                MemberInfo memberInfo = callstack[i];
                baseObject = this.GetValueFromMember(baseObject, memberInfo);
                list.Add(memberInfo);
                if (baseObject == null)
                {
                    return null;
                }
                System.Type type = baseObject.GetType();
                if (!IsValueType(type) && (type != typeof(string)))
                {
                    list.Clear();
                    this.m_CallingObjectRef = baseObject;
                }
            }
            this.m_Callstack = list.ToArray();
            return baseObject;
        }

        private object GetValueFromMember(object obj, MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo)
            {
                return (memberInfo as FieldInfo).GetValue(obj);
            }
            if (!(memberInfo is MethodInfo))
            {
                throw new InvalidPathException(memberInfo.Name);
            }
            return (memberInfo as MethodInfo).Invoke(obj, null);
        }

        private static bool IsValueType(System.Type type)
        {
            return type.IsValueType;
        }

        public static bool TryGetMemberType(GameObject gameObject, string path, out System.Type value)
        {
            try
            {
                value = new MemberResolver(gameObject, path).GetMemberType();
                return true;
            }
            catch (InvalidPathException)
            {
                value = null;
                return false;
            }
        }

        public static bool TryGetValue(GameObject gameObject, string path, out object value)
        {
            try
            {
                value = new MemberResolver(gameObject, path).GetValue(false);
                return true;
            }
            catch (InvalidPathException)
            {
                value = null;
                return false;
            }
        }

        private void ValidatePath(string path)
        {
            bool flag = false;
            if (path.StartsWith(".") || path.EndsWith("."))
            {
                flag = true;
            }
            if (path.IndexOf("..") >= 0)
            {
                flag = true;
            }
            if (Regex.IsMatch(path, @"\s"))
            {
                flag = true;
            }
            if (flag)
            {
                throw new InvalidPathException(path);
            }
        }
    }
}

