import { Container, Header, Space, Table, createStyles } from "@mantine/core";
import { faPencil } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate } from "react-router-dom";
import { routes } from "../../routes";
import { useEffect, useState } from "react";

import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { ApiResponse, MessagesGetDto } from "../../constants/types";
export {};
export const MessageListing = () => {
  const [messages, setMessages] = useState<MessagesGetDto[]>();
  const navigate = useNavigate();
  const { classes } = useStyles();
  useEffect(() => {
    fetchMessages();

    async function fetchMessages() {
      const response = await api.get<ApiResponse<MessagesGetDto[]>>(
        "/api/Messages"
      );

      if (response.data.hasErrors) {
        showNotification({ message: "Error fetching Messages." });
      }
      if (response.data.data) {
        setMessages(response.data.data);
      }
    }
  }, []);

  return (
    <Container>
      <Header height={32}>Messages</Header>
      <Space h="md" />
      {messages && (
        <Table withBorder striped>
          <thead>
            <tr>
              <th></th>
              <th>Content</th>
              <th>CreatedAt</th>
            </tr>
          </thead>
          <tbody>
            {messages.map((message) => {
              return (
                <tr>
                  <td>
                    <FontAwesomeIcon
                      className={classes.iconButton}
                      icon={faPencil}
                      onClick={() => {
                        navigate(
                          routes.MessageUpdate.replace(":id", `${message.id}`)
                        );
                      }}
                    />
                  </td>
                  <td>{message.content}</td>
                  <td>{message.createdAt}</td>
                </tr>
              );
            })}
          </tbody>
        </Table>
      )}
    </Container>
  );
};
const useStyles = createStyles(() => {
  return {
    iconButton: {
      cursor: "pointer",
    },
  };
});
