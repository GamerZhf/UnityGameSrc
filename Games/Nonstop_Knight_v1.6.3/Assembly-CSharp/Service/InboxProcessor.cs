namespace Service
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    public class InboxProcessor : MonoBehaviour
    {
        private Dictionary<string, System.Type> m_commandIdMapping = new Dictionary<string, System.Type>();
        private ICommandProcessor m_commandProcessor;

        private System.Type getInboxCommandClass(string inboxCommandId)
        {
            if (this.m_commandIdMapping.ContainsKey(inboxCommandId.ToLower()))
            {
                return this.m_commandIdMapping[inboxCommandId.ToLower()];
            }
            return null;
        }

        public void Initialize()
        {
            this.setupCommandIdMapping();
        }

        protected void OnDisable()
        {
            Service.Binder.EventBus.OnInboxCommands -= new Service.Events.InboxCommands(this.onInboxCommands);
        }

        protected void OnEnable()
        {
            Service.Binder.EventBus.OnInboxCommands += new Service.Events.InboxCommands(this.onInboxCommands);
        }

        private void onInboxCommands(List<InboxCommand> inboxCommands)
        {
            inboxCommands.ForEach(delegate (InboxCommand inboxCommand) {
                System.Type type = this.getInboxCommandClass(inboxCommand.CommandId.ToString());
                if (type != null)
                {
                    object[] args = new object[] { inboxCommand.Parameters };
                    ((IInboxCommandHandler) Activator.CreateInstance(type, args)).Execute();
                }
                else
                {
                    Debug.Log("Unknown InboxCommand " + inboxCommand.CommandId);
                }
            });
        }

        private void setupCommandIdMapping()
        {
            foreach (System.Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass)
                {
                    foreach (object obj2 in type.GetCustomAttributes(typeof(InboxCommandAttribute), true))
                    {
                        InboxCommandAttribute attribute = (InboxCommandAttribute) obj2;
                        this.m_commandIdMapping.Add(attribute.CommandId.ToString().ToLower(), type);
                    }
                }
            }
        }
    }
}

