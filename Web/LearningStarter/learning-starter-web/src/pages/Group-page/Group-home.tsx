//poop
import { faArrowLeft, faPen, faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Container, createStyles, Title, Tabs, Button, Menu } from "@mantine/core";
import { showNotification } from "@mantine/notifications";
import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../../config/axios";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { routes } from "../../routes";
import { icon } from "@fortawesome/fontawesome-svg-core";

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
      {/* Back Button */}
      <Button
        onClick={() => {
          navigate(routes.GroupListing);
        }}
        style={{
          backgroundColor: 'transparent',
          border: 'none',
          cursor: 'pointer',
          position: 'absolute',
          top: '80px',
          left: '80px',
        }}
      >
        <FontAwesomeIcon icon={faArrowLeft} size="xl" />
      </Button>

      {/* Group Title */}
      <Title order={1} align="center" style={{ marginBottom: '20px' }}>
        {group?.groupName || 'Loading...'}
      </Title>

      {/* Tabs */}
      <Tabs  color ="teal"defaultValue="Tests">
        <Tabs.List grow >
          <Tabs.Tab value="Tests">Tests</Tabs.Tab>
          <Tabs.Tab value="Flashcard Sets">Flashcard Sets</Tabs.Tab>
          <Tabs.Tab value="Assignments">Assignments</Tabs.Tab>
        </Tabs.List>

        <Tabs.Panel value="Tests">
          {/* Tests Content */}
          
              {group?.tests.map((test) => (
                  <div
                    style={{
                      whiteSpace: 'nowrap',
                      cursor: 'pointer',
                      display: 'flex',
                      alignItems: 'center',
                    }}
                  >
                    <Button 
                    variant="subtle" 
                    color="gray" 
                    size="sm" 
                    radius="xs" 
                    onClick={ () => navigate(routes.TestingPage.replace(":id", `${test.id}`))}> 
                    {test.testName}
                    </Button>
                   
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
              ))}
            <Button 
            variant="subtle" 
            color="gray" 
            size="sm" 
            radius="xs" 
            onClick={ () => navigate(routes.TestCreate.replace(":id", `${group?.id}`))}>
              Create Test
            </Button>  
        </Tabs.Panel>

        <Tabs.Panel value="Flashcard Sets">
          {/* Flashcard Sets Content */}
         
              {group?.flashCardSets.map((flashCardSet) => (
                  <div
                    style={{
                      whiteSpace: 'nowrap',
                      cursor: 'pointer',
                      display: 'flex',
                      alignItems: 'center',
                    }}
                  >
                     <Button 
                    variant="subtle" 
                    color="gray" 
                    size="sm" 
                    radius="xs" 
                    onClick={ () => navigate(routes.FlashCardListing.replace(":id", `${flashCardSet.id}`))}> 
                    {flashCardSet.setName}
                    </Button>

                    <span style={{ marginRight: '8px' }}></span>
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
              ))}
                 <Button 
            variant="subtle" 
            color="gray" 
            size="sm" 
            radius="xs" 
            onClick={ () => navigate(routes.FCSetCreate.replace(":id", `${group?.id}`))}>
              Create Set
            </Button> 
        </Tabs.Panel>

        <Tabs.Panel value="Assignments">
          {/* Assignments Content */}
         
              {group?.assignments.map((assignment) => (
                  <div
                    style={{
                      whiteSpace: 'nowrap',
                      cursor: 'pointer',
                      display: 'flex',
                      alignItems: 'center',
                    }}
                  >
                      <Button 
                    variant="subtle" 
                    color="gray" 
                    size="sm" 
                    radius="xs" 
                    onClick={ () => navigate(routes.AssignmentGradeListingg.replace(":id", `${assignment.id}`))}> 
                    {assignment.assignmentName}
                    </Button>

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
              ))}
                 <Button 
            variant="subtle" 
            color="gray" 
            size="sm" 
            radius="xs" 
            onClick={ () => navigate(routes.AssignmentCreatee.replace(":id", `${group?.id}`))}>
              Create Assignment
            </Button>
        </Tabs.Panel>
      </Tabs>

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
