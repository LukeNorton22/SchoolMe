// GroupChat.tsx

import React, { useEffect, useState } from "react";
import { Input, Button } from "@mantine/core";
import { useParams } from "react-router-dom";
import api from "../../config/axios";
import { MessagesGetDto } from "../../constants/types"; 

interface GroupChatProps {
  groupId: string;
}

const GroupChat: React.FC<GroupChatProps> = ({ groupId }) => {
  const [messages, setMessages] = useState<MessagesGetDto[]>([]);
  const [newMessage, setNewMessage] = useState<string>("");
  const { id } = useParams();

  const fetchMessages = async () => {
    try {
      const response = await api.get<MessagesGetDto[]>(`/api/Message/${groupId}`);
      setMessages(response.data);
    } catch (error) {
      console.error("Error fetching messages:", error);
    }
  };

  const sendMessage = async () => {
    try {
      await api.post<MessagesGetDto>(`/api/Message/${groupId}`, {
        content: newMessage,
      });
      setNewMessage("");
      fetchMessages(); // Refresh messages after sending a new one
    } catch (error) {
      console.error("Error sending message:", error);
    }
  };

  useEffect(() => {
    fetchMessages();
  }, [groupId]);

  return (
    <div>
      <div>
        {messages.map((message) => (
          <div key={message.id}>
            {message.content}
          </div>
        ))}
      </div>
      <div>
        <Input
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
        />
        <Button onClick={sendMessage}>Send</Button>
      </div>
    </div>
  );
};

export default GroupChat;
