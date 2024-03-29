package com.net.client.service;

import com.cell.net.io.MessageHeader;

public interface WaitingListener<REQ extends MessageHeader, RSP extends MessageHeader>
{
	/**
	 * @param service
	 * @param request
	 * @param response
	 */
	public void response(BasicNetService service, REQ request, RSP response);
	
	/**
	 * @param service
	 * @param request
	 * @param send_time 该消息的发送时间
	 */
	public void timeout(BasicNetService service, REQ request, long send_time);
}
