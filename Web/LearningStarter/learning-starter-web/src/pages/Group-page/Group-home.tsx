import { faPen, faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Container, Menu, Button, createStyles, Title } from "@mantine/core";
import { showNotification } from "@mantine/notifications";
import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../../config/axios";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { routes } from "../../routes";

export const GroupHome = () => {
  const { id } = useParams();
  const { classes } = useStyles();
  const navigate = useNavigate();
  const [group, setGroup] = useState<GroupGetDto | null>(null);

  const fetchGroup = async () => {
    try {
      const response = await api.get<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`);
      setGroup(response.data.data);
    } catch (error) {
      console.error('Error fetching group:', error);
    }
  };

  const handleTestDelete = async (testId: number, groupId: number) => {
    try {
      await api.delete(`/api/Tests/${testId}`);
      showNotification({ message: `Test has entered the trash` });
    } catch (error) {
      console.error('Error deleting test:', error);
      showNotification({ title: 'Error', message: 'Failed to delete the test' });
    }
  };

  const handleAssignmentDelete = async (assignmentId: number, groupId: number) => {
    try {
      await api.delete(`/api/assignments/${assignmentId}`);
      showNotification({ message: `Assignment has entered the trash` });
    } catch (error) {
      console.error('Error deleting assignment:', error);
      showNotification({ title: 'Error', message: 'Failed to delete the assignment.' });
    }
  };

  const handleFcSetDelete = async (fcSetId: number, groupId: number) => {
    try {
      await api.delete(`/api/FCSets/${fcSetId}`);
      showNotification({ message: `Flashcard set has entered the trash` });
    } catch (error) {
      console.error('Error deleting flashcard set:', error);
      showNotification({ title: 'Error', message: 'Failed to delete the flashcard set.' });
    }
  };

  const handleDeleteAndNavigate = async (itemId: number, groupId: number, itemType: string) => {
    switch (itemType) {
      case 'test':
        await handleTestDelete(itemId, groupId);
        break;
      case 'assignment':
        await handleAssignmentDelete(itemId, groupId);
        break;
      case 'fcSet':
        await handleFcSetDelete(itemId, groupId);
        break;
      default:
        return;
    }

    fetchGroup(); // Ensure that the group is updated after deletion
    navigate(routes.GroupHome.replace(":id", `${groupId}`));
  };

  useEffect(() => {
    fetchGroup();
  }, [id]);

  return (
    <Container>
      {/* Group Title */}
      <Title order={1}  align="center" style={{ marginBottom: '20px' }}>
        {group?.groupName || 'Loading...'}
      </Title>
      {/* Tests Menu */}
      <Menu trigger="hover" openDelay={100} closeDelay={400}>
        <Menu.Target>
          <Button size="sm" color="transparent" style={{ border: 'none', marginRight: '8px' }}>
            Tests
          </Button>
        </Menu.Target>
        <Menu.Dropdown>
          {group?.tests.map((test) => (
            <Menu.Item key={test.id}>
              <div
                style={{
                  whiteSpace: 'nowrap',
                  cursor: 'pointer',
                  display: 'flex',
                  alignItems: 'center',
                }}
              >
                <span style={{ marginRight: '8px' }}>{test.testName}</span>
                <FontAwesomeIcon
                  className={classes.iconButton}
                  icon={faPen}
                  onClick={() => navigate(routes.TestUpdate.replace(":id", `${test.id}`))}
                />
                <FontAwesomeIcon
                  className={classes.iconButton}
                  icon={faTrash}
                  color="red"
                  size="sm"
                  onClick={() => handleDeleteAndNavigate(test.id, test.groupId, 'test')}
                  style={{ cursor: 'pointer', marginLeft: '8px' }}
                />
              </div>
            </Menu.Item>
          ))}
          <Button
            size="sm"
            color="transparent"
            style={{ border: 'none', marginTop: '8px' }}
            onClick={() => navigate(routes.TestCreate.replace(":id", `${group?.id}`))}
          >
            Create New Test
          </Button>
        </Menu.Dropdown>
      </Menu>

      {/* Flash Card Sets Menu */}
      <Menu trigger="hover" openDelay={100} closeDelay={400}>
        <Menu.Target>
          <Button size="sm" color="transparent" style={{ border: 'none', marginRight: '8px' }}>
            Flash Card Sets
          </Button>
        </Menu.Target>
        <Menu.Dropdown>
          {group?.flashCardSets.map((flashCardSet) => (
            <Menu.Item key={flashCardSet.id}>
              <div
                style={{
                  whiteSpace: 'nowrap',
                  cursor: 'pointer',
                  display: 'flex',
                  alignItems: 'center',
                }}
              >
                <span style={{ marginRight: '8px' }}>{flashCardSet.setName}</span>
                <FontAwesomeIcon
                  className={classes.iconButton}
                  icon={faPen}
                  onClick={() => navigate(routes.FlashCardSetUpdate.replace(":id", `${flashCardSet.id}`))}
                />
                <FontAwesomeIcon
                  className={classes.iconButton}
                  icon={faTrash}
                  color="red"
                  size="sm"
                  onClick={() => handleDeleteAndNavigate(flashCardSet.id, flashCardSet.groupId, 'fcSet')}
                  style={{ cursor: 'pointer', marginLeft: '8px' }}
                />
              </div>
            </Menu.Item>
          ))}
          <Button
            size="sm"
            color="transparent"
            style={{ border: 'none', marginTop: '8px' }}
            onClick={() => navigate(routes.FCSetCreate.replace(":id", `${group?.id}`))}
          >
            Create New Flash Card Set
          </Button>
        </Menu.Dropdown>
      </Menu>

      {/* Assignments Menu */}
      <Menu trigger="hover" openDelay={100} closeDelay={400}>
        <Menu.Target>
          <Button size="sm" color="transparent" style={{ border: 'none', marginRight: '8px' }}>
            Assignments
          </Button>
        </Menu.Target>
        <Menu.Dropdown>
          {group?.assignments.map((assignment) => (
            <Menu.Item key={assignment.id}>
              <div
                style={{
                  whiteSpace: 'nowrap',
                  cursor: 'pointer',
                  display: 'flex',
                  alignItems: 'center',
                }}
              >
                <span style={{ marginRight: '8px' }}>{assignment.assignmentName}</span>
                <FontAwesomeIcon
                  className={classes.iconButton}
                  icon={faPen}
                  onClick={() => navigate(routes.AssignmentUpdate.replace(":id", `${assignment.id}`))}
                />
                <FontAwesomeIcon
                  className={classes.iconButton}
                  icon={faTrash}
                  color="red"
                  size="sm"
                  onClick={() => handleDeleteAndNavigate(assignment.id, assignment.groupId, 'assignment')}
                  style={{ cursor: 'pointer', marginLeft: '8px' }}
                />
              </div>
            </Menu.Item>
          ))}
          <Button
            size="sm"
            color="transparent"
            style={{ border: 'none', marginTop: '8px' }}
            onClick={() => navigate(routes.AssignmentCreatee.replace(":id", `${group?.id}`))}
          >
            Create New Assignment
          </Button>
        </Menu.Dropdown>
      </Menu>

      {/* The rest of your code... */}
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
