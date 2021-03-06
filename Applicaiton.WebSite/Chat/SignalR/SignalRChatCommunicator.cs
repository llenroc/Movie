﻿using Application.Chat;
using Application.Chat.Dto;
using Application.Friendships;
using Application.Friendships.Dto;
using Castle.Core.Logging;
using Infrastructure;
using Infrastructure.AutoMapper;
using Infrastructure.Dependency;
using Infrastructure.RealTime;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebSite.Chat.SignalR
{
    public class SignalRChatCommunicator : IChatCommunicator, ITransientDependency
    {
        /// <summary>
        /// Reference to the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        private static IHubContext ChatHub
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            }
        }

        public SignalRChatCommunicator()
        {
            Logger = NullLogger.Instance;
        }

        public void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);

                if (signalRClient == null)
                {
                    return;
                }
                signalRClient.getChatMessage(message.MapTo<ChatMessageDto>());
            }
        }

        public void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friendship, bool isOwnRequest, bool isFriendOnline)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);

                if (signalRClient == null)
                {
                    return;
                }
                var friendshipRequest = friendship.MapTo<FriendDto>();
                friendshipRequest.IsOnline = isFriendOnline;

                signalRClient.getFriendshipRequest(friendshipRequest, isOwnRequest);
            }
        }

        public void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);

                if (signalRClient == null)
                {
                    continue;
                }
                signalRClient.getUserConnectNotification(user, isConnected);
            }
        }

        public void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);

                if (signalRClient == null)
                {
                    continue;
                }
                signalRClient.getUserStateChange(user, newState);
            }
        }

        public void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user)
        {
            foreach (var client in clients)
            {
                var signalRClient = GetSignalRClientOrNull(client);

                if (signalRClient == null)
                {
                    continue;
                }
                signalRClient.getallUnreadMessagesOfUserRead(user);
            }
        }

        private dynamic GetSignalRClientOrNull(IOnlineClient client)
        {
            var signalRClient = ChatHub.Clients.Client(client.ConnectionId);

            if (signalRClient == null)
            {
                Logger.Debug("Can not get chat user " + client.UserId + " from SignalR hub!");
                return null;
            }
            return signalRClient;
        }
    }
}