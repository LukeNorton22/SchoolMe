//poop
import {
  faArrowLeft,  faPen, faPencil, faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  Container, Title, Tabs, Button, Table, Space, Input,
} from "@mantine/core";
import { showNotification } from "@mantine/notifications";
import { useState, useEffect, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../../config/axios";
import { GroupGetDto, ApiResponse, MessagesGetDto } from "../../constants/types";
import { routes } from "../../routes";
import { createStyles } from "@mantine/core";
import { useUser } from "../../authentication/use-auth";
import { UpdateDeleteButton } from './three-dots'; // Import the UpdateDeleteButton component
import {  faChevronDown } from '@fortawesome/free-solid-svg-icons';



export const GroupHome = () => {
  const { id, userId} = useParams();
  const { classes } = useStyles();
  const navigate = useNavigate();
  const [group, setGroup] = useState<GroupGetDto | null>(null);
  const [newMessage, setNewMessage] = useState("");
  const tableRef = useRef<HTMLDivElement | null>(null);
  const {  theme } = useStyles();
  const user = useUser();

  const fetchGroup = async () => {
    try {
      const response = await api.get<ApiResponse<GroupGetDto>>(
        `/api/Groups/${id}`
      );
      setGroup(response.data.data);
    } catch (error) {
      console.error("Error fetching group:", error);
    }
  };

  const handleTestDelete = async (testId: number, groupId: number) => {
    try {
      await api.delete(`/api/Tests/${testId}`);
      showNotification({ message: `Test has entered the trash` });
    } catch (error) {
      console.error("Error deleting test:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the test",
      });
    }
  };
  const handleMessageDelete = async (messageId: number, groupId: number) => {
    try {
      await api.delete(`/api/Message/${messageId}`);
      showNotification({ message: `Message has entered the trash` });
    } catch (error) {
      console.error("Error deleting message:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the message",
      });
    }
  };

  const handleAssignmentDelete = async (
    assignmentId: number,
    groupId: number
  ) => {
    try {
      await api.delete(`/api/assignments/${assignmentId}`);
      showNotification({ message: `Assignment has entered the trash` });
    } catch (error) {
      console.error("Error deleting assignment:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the assignment.",
      });
    }
  };

  const handleFcSetDelete = async (fcSetId: number, groupId: number) => {
    try {
      await api.delete(`/api/FCSets/${fcSetId}`);
      showNotification({ message: `Flashcard set has entered the trash` });
    } catch (error) {
      console.error("Error deleting flashcard set:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the flashcard set.",
      });
    }
  };

  const handleDeleteAndNavigate = async (
    itemId: number,
    groupId: number,
    itemType: string
  ) => {
    switch (itemType) {
      case "test":
        await handleTestDelete(itemId, groupId);
        break;
        case "message":
        await handleMessageDelete(itemId, groupId);
        break;
      case "assignment":
        await handleAssignmentDelete(itemId, groupId);
        break;
      case "fcSet":
        await handleFcSetDelete(itemId, groupId);
        break;
      default:
        return;
    }

    fetchGroup(); // Ensure that the group is updated after deletion
    navigate(routes.GroupHome.replace(":id", `${groupId}`));
  };

  console.log(user);

  const handleSendMessage = async () => {
    try {
      console.log("user.userName:", user.userName);

      console.log("userId:", user.id);

      const userId = user.id;

      // Assuming you have an API endpoint to send messages
      await api.post(`/api/Message/${id}/${userId}`, {
        groupId: group?.id,
        content: newMessage,
        userId: user.id,
        userName: user.userName
      });
      // Fetch the updated group information, including the new message
     
      setNewMessage("");
      fetchGroup(); // Refresh the messages after sending
      scrollTableToBottom(); // After sending a message, scroll the table to the bottom

    } catch (error) {
      console.error("Error sending message:", error);
      showNotification({
        title: "Error",
        message: "Failed to send the message",
      });
    }

  };



  useEffect(() => {
    fetchGroup();
  }, [id]);

  useEffect(() => {
    scrollTableToBottom(); // Scroll to the bottom when the component mounts or when messages change
  }, [group?.messages]);

  const scrollTableToBottom = () => {
    if (tableRef.current) {
      tableRef.current.scrollTop = tableRef.current.scrollHeight;
    }
  };

  console.log("Group messages:", group?.messages);
  console.log("user:", user);


  return (
    <Container style={{ width: "220%" }}>
    {/* Back Button */}
      <Button
        onClick={() => {
          navigate(routes.GroupListing);
        }}
        style={{
          backgroundColor: "transparent",
          border: "none",
          cursor: "pointer",
          position: "absolute",
          top: "80px",
          left: "80px",
        }}
      >
        <FontAwesomeIcon icon={faArrowLeft} size="xl" />
      </Button>

      {/* Group Title */}
      <Title order={1} align="center" style={{ marginBottom: "20px" }}>
        {group?.groupName || "Loading..."}
      </Title>

      {/* Tabs */}
      <Tabs orientation = "horizontal" color="teal" defaultValue="Chat">
      <Tabs.List grow>
          <Tabs.Tab value="Chat">Chat</Tabs.Tab>
          <Tabs.Tab value="Tests">Tests</Tabs.Tab>
          <Tabs.Tab value="Flashcard Sets">Flashcard Sets</Tabs.Tab>
          <Tabs.Tab value="Assignments">Assignments</Tabs.Tab>
          <Tabs.Tab value="GroupUser">Group Members</Tabs.Tab>
        </Tabs.List>

  <Tabs.Panel value="Tests">
  {/* Tests Content */}
  {group?.tests.map((test) => (
    <div
      style={{
        whiteSpace: "nowrap",
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
      }}
      key={test.id}
    >
      <Button
        variant="subtle"
        color="gray"
        size="sm"
        radius="xs"
        onClick={() =>
          navigate(routes.TestingPage.replace(":id", `${test.id}`))
        }
      >
        {test.testName}
      </Button>

      {user.id === test.userId && (
        <UpdateDeleteButton
          onUpdate={() =>
            navigate(routes.TestUpdate.replace(":id", `${test.id}`))
          }
          onDelete={() =>
            handleDeleteAndNavigate(test.id, test.groupId, "test")
          }
        />
      )}
    </div>
  ))}

  <Button
    variant="subtle"
    color="gray"
    size="sm"
    radius="xs"
    onClick={() =>
      navigate(routes.TestCreate.replace(":id", `${group?.id}`))
    }
  >
    Create Test
  </Button>
</Tabs.Panel>


        <Tabs.Panel value="GroupUser">
  {/* Users List */}
  {group?.users.map((user) => (
    <div
      key={user.id} // Make sure to provide a unique key for each item in the list
      style={{
        whiteSpace: "nowrap",
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
        marginBottom: 8, // Add some spacing between users
      }}
    >
      <Button
        variant="subtle"
        color="gray"
        size="sm"
        radius="xs"
        onClick={() => navigate(routes.TestingPage.replace(":id", `${user.id}`))}
      >
        {user.userName}
      </Button>
    </div>
  ))}

  {/* Add User Button */}
  <Button
    variant="subtle"
    color="gray"
    size="sm"
    radius="xs"
    onClick={() =>
      navigate(routes.GroupUserCreate.replace(":groupId", `${group?.id}`))
    }
  >
    Add User
  </Button>
</Tabs.Panel>
<Tabs.Panel value="Flashcard Sets">
  {/* Flashcard Sets Content */}
  {group?.flashCardSets.map((flashCardSet) => (
    <div
      key={flashCardSet.id}
      style={{
        whiteSpace: "nowrap",
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
      }}
    >
      <Button
        variant="subtle"
        color="gray"
        size="sm"
        radius="xs"
        onClick={() =>
          navigate(routes.FlashCardSetListing.replace(":id", `${flashCardSet.id}`))
        }
      >
        {flashCardSet.setName}
      </Button>

      <span style={{ marginRight: "8px" }}></span>

      {user.id === flashCardSet.userId && (
        <UpdateDeleteButton
          onUpdate={() =>
            navigate(routes.FlashCardSetUpdate.replace(":id", `${flashCardSet.id}`))
          }
          onDelete={() =>
            handleDeleteAndNavigate(flashCardSet.id, flashCardSet.groupId, "fcSet")
          }
        />
      )}
    </div>
  ))}

  <Button
    variant="subtle"
    color="gray"
    size="sm"
    radius="xs"
    onClick={() =>
      navigate(routes.FCSetCreate.replace(":id", `${group?.id}`))
    }
  >
    Create Set
  </Button>
</Tabs.Panel>


<Tabs.Panel value="Assignments">
  {/* Assignments Content */}
  {group?.assignments.map((assignment) => (
    <div
      style={{
        whiteSpace: "nowrap",
        cursor: "pointer",
        display: "flex",
        alignItems: "center",
      }}
      key={assignment.id}
    >
      <Button
        variant="subtle"
        color="gray"
        size="sm"
        radius="xs"
        onClick={() =>
          navigate(
            routes.AssignmentListing.replace(":id", `${assignment.id}`)
          )
        }
      >
        {assignment.assignmentName}
      </Button>

      {user.id === assignment.userId && (
        <UpdateDeleteButton
          onUpdate={() =>
            navigate(
              routes.AssignmentUpdate.replace(":id", `${assignment.id}`)
            )
          }
          onDelete={() =>
            handleDeleteAndNavigate(
              assignment.id,
              assignment.groupId,
              "assignment"
            )
          }
        />
      )}
    </div>
  ))}

  <Button
    variant="subtle"
    color="gray"
    size="sm"
    radius="xs"
    onClick={() =>
      navigate(routes.AssignmentCreatee.replace(":id", `${group?.id}`))
    }
  >
    Create Assignment
  </Button>
</Tabs.Panel>

     
        <Tabs.Panel value="Chat">
      {group && (
        <div style={{ position: 'fixed', bottom: 70, left: 170, right: 170, padding: '0px', backgroundColor: '' }}>
          <Input
            size="md"
            placeholder="Type your message..."
            value={newMessage}
            onChange={(e) => setNewMessage(e.target.value)}
            style={{ borderColor: theme.colors.teal[6], color: theme.black }}
          />
          <Space h={8}></Space>
          <Button variant="filled" color="teal" onClick={handleSendMessage}>
            Send
          </Button>
        </div>
      )}

      <div style={{ left: "120", right: "120", maxHeight: "385px", overflowY: "auto", width: "100%" }} ref={tableRef}>
        {/* Set your desired max height */}
        {group && (
          <Table style={{ borderColor: theme.colors.teal[6], width: "100%", tableLayout: "fixed" }}>
            <colgroup>
              <col style={{ width: "90%" }} /> {/* Move the first column style to the right */}
              <col style={{ width: "10%" }} /> {/* Move the second column style to the left */}
            </colgroup>
            <tbody>
              {group.messages.map((message) => (
                <tr key={message.id}>
                  <td style={{ textAlign: 'left', wordWrap: 'break-word' }}>
                    <strong style={{ fontSize: '1.2em' }}>{message.userName}</strong>
                    <Space></Space>{message.content}<Space></Space>{message.createdAt}
                  </td>
                  <td style={{ textAlign: 'right' }}>
                    {user.id === message.userId && ( // Check if the current user is the sender
                      <UpdateDeleteButton
                        onUpdate={() => navigate(routes.MessageUpdate.replace(":id", `${message.id}`))}
                        onDelete={() => handleDeleteAndNavigate(message.id, message.groupId, "message")}
                      />
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </Table>
        )}
      </div>
    </Tabs.Panel>

      </Tabs>
    </Container>
   
  );
};

const useStyles = createStyles((theme) => {
  return {
    iconButton: {
      cursor: "pointer",
    },
  };
});
