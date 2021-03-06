﻿/*
 Copyright (c) 2012-2013 Clint Banzhaf
 This file is part of "Meridian59 .NET".

 "Meridian59 .NET" is free software: 
 You can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 either version 3 of the License, or (at your option) any later version.

 "Meridian59 .NET" is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 See the GNU General Public License for more details.

 You should have received a copy of the GNU General Public License along with "Meridian59 .NET".
 If not, see http://www.gnu.org/licenses/.
*/

using Android.Widget;
using Android.Views;
using Android.App;
using System.ComponentModel;
using Meridian59.Data.Models;
using Meridian59.Data.Lists;
using Meridian59.Common.Enums;
using Android.Graphics;

namespace Meridian59.Android.ChatClient.Adapters
{
    /// <summary>
    /// Adapter for BaseList with ChatMessages
    /// </summary>
    public class ChatMessageAdapter : BaseAdapter<ChatMessage>
    {
        protected Activity context;
        protected readonly BaseList<ChatMessage> chat;

        public ChatMessageAdapter(BaseList<ChatMessage> Chat, Activity Activity)
            : base()
        {
            chat = Chat;
            context = Activity;

            chat.ListChanged += chat_ListChanged;
        }

        public override ChatMessage this[int position]
        {
            get { return chat[position]; }
        }

        public override int Count
        {
            get { return chat.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ChatMessage item = chat[position];
            View view = convertView;
            
            if (convertView == null || !(convertView is LinearLayout))
                view = context.LayoutInflater.Inflate(Resource.Layout.ChatItemView, parent, false);

            TextView text = view.FindViewById<TextView>(Resource.Id.textItem);
            text.Text = item.FullString;

            switch (item.ChatMessageType)
            {
                case ChatMessageType.ServerChatMessage:
                    text.SetTextColor(Color.Purple);
                    break;

                case ChatMessageType.SystemMessage:
                    text.SetTextColor(Color.LightBlue);
                    break;

                case ChatMessageType.ObjectChatMessage:
                    text.SetTextColor(Color.White);
                    break;
            }

            return view;
        }

        protected void chat_ListChanged(object sender, ListChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }
    }
}
