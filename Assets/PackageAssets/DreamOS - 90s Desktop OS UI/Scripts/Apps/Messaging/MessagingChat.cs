﻿using System.Collections.Generic;
using UnityEngine;

namespace Michsky.DreamOS
{
    [CreateAssetMenu(fileName = "New Chat", menuName = "DreamOS/New Messaging Chat")]
    public class MessagingChat : ScriptableObject
    {
        // Settings
        public bool saveConversation = false;
        public bool useDynamicMessages = false;
        public bool useStoryTeller = false;

        // List
        public List<ChatMessage> messageList = new List<ChatMessage>();
        public List<DynamicMessages> dynamicMessages = new List<DynamicMessages>();
        public List<StoryTeller> storyTeller = new List<StoryTeller>();

        public enum DynamicMessageReplyBehavior { DoNothing, DisableReply }

        [System.Serializable]
        public class ChatMessage
        {
            [TextArea(3, 6)] public string messageContent = "My message";
            public ObjectType objectType;
            public MessageAuthor messageAuthor;
            public string sentTime = "00:00";
            public AudioClip audioMessage;
            public Sprite imageMessage;

            [Header("Localization")]
            public string messageKey;
        }

        [System.Serializable]
        public class DynamicMessages
        {
            public string messageID = "MESSAGE_0";
            [TextArea(3, 6)] public string messageContent = "My message";
            [TextArea(3, 6)] public string replyContent = "Reply message";

            [Header("Settings")]
            [Range(0.1f, 25)] public float replyLatency = 1;
            [Range(0.1f, 25)] public float replyTimer = 1.5f;
            public DynamicMessageReplyBehavior replyBehavior = DynamicMessageReplyBehavior.DoNothing;
            public bool enableReply = true;

            [Header("Storyteller")]
            public string runStoryteller;

            [Header("Localization")]
            public string replyKey;
        }

        [System.Serializable]
        public class StoryTeller
        {
            public string itemID = "ITEM_0";
            public MessageAuthor messageAuthor;
            [TextArea(3, 6)] public string messageContent = "My message";
            [Range(0, 25)] public float messageLatency = 1;
            [Range(0, 25)] public float messageTimer = 1.5f;
            public List<StoryTellerItem> replies = new List<StoryTellerItem>();

            [Header("Localization")]
            public string messageKey;
        }

        [System.Serializable]
        public class StoryTellerItem
        {
            public string replyID;
            [TextArea] public string replyBrief = "Reply brief";
            [TextArea] public string replyContent = "Reply content";
            [TextArea] public string replyFeedback = "Reply feedback";
            [Range(0.1f, 25)] public float feedbackLatency = 1;
            [Range(0.1f, 25)] public float feedbackTimer = 1.5f;
            public string callAfter;

            [Header("Localization")]
            public string briefKey;
            public string contentKey;
            public string feedbackKey;
        }

        public enum MessageAuthor { Self, Individual }
        public enum ObjectType { Message, Date, AudioMessage, ImageMessage }
    }
}