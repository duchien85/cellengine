package com.net.server;

import com.net.MessageHeader;

public interface ChannelListener 
{
	public void receivedMessage(Channel channel, ClientSession sender, MessageHeader message);
	
	public void sessionLeaved(Channel channel, ClientSession session);
	
	public void sessionJoined(Channel channel, ClientSession session);
}
