import React, { useEffect, useState } from "react";
import {  useNavigate, useParams } from "react-router-dom";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { Button, Center, Container, Space, Title, createStyles,  } from "@mantine/core";
import api from "../../config/axios";
import { routes } from "../../routes";
import { faArrowLeft, faPencil, faPlus, faTrash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { showNotification } from "@mantine/notifications";


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

  const handleTestDelete = async (testId: number) => {
    try {
      await api.delete(`/api/Tests/${testId}`);
      showNotification({  message: `Test has entered the trash` });
      fetchGroup();
    } catch (error) {
      console.error('Error deleting test:', error);
      showNotification({ title: 'Error', message: 'Failed to delete the test' });
    }
  };
  const handleFcSetDelete = async (fcSetId: number) => {
    try {
      await api.delete(`/api/FCSets/${fcSetId}`);
      showNotification({  message: `Flashcard set has entered the trash` });
      fetchGroup();
    } catch (error) {
      console.error('Error deleting flashcard set:', error);
      showNotification({ title: 'Error', message: 'Failed to delete the flashcard set.' });
    }
  };

  const handleAssignmentDelete = async (assignmentId: number) => {
    try {
      await api.delete(`/api/assignments/${assignmentId}`);
      showNotification({ message: `Assignment has entered the trash` });
      fetchGroup();
    } catch (error) {
      console.error('Error deleting assignment:', error);
      showNotification({ title: 'Error', message: 'Failed to delete the assignment.' });
    }
  };

  useEffect(() => {
    fetchGroup();
  }, [id]);

  return (
    <Container>
      {group && (
        <div>
          <Button
            onClick={() => navigate(routes.GroupListing)}
            style={{
              backgroundColor: 'transparent',
              border: 'none',
              cursor: 'pointer',
            }}
          >
            <FontAwesomeIcon icon={faArrowLeft} size="xl" /> 
          </Button> 
         <Center>
          <Title>{group.groupName}</Title>
          </Center>
          <h1>Tests</h1>
          <ul>
            {group.tests.map((test) => (
              <li key={test.id}>
                <Button onClick={() => {
                 navigate(routes.TestingPage.replace(":id", `${test.id}`))
                 }}
                > 
                 {test.testName}  
                 </Button> 
                 <FontAwesomeIcon
                      className={classes.iconButton}
                      icon={faPencil}
                      onClick={() => {
                        navigate(
                          routes.TestUpdate.replace(":id", `${test.id}`)
                        );
                      }}
                    />
                     <Button onClick={() => handleTestDelete(test.id)} color="red" variant="outline">
                  <FontAwesomeIcon icon={faTrash} />
                </Button>
                <Space h="md" />
                    
              </li>
            ))           
            }
            <Button
                  onClick={() => {
                  navigate(routes.TestCreate.replace(":id", `${group.id}`));
                  }}
                >
                <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
                  New Test
                </Button>
          
          </ul>
          <h1>Flash Card Sets</h1>
          <ul>
            {group.flashCardSets.map((flashCardSet) => (
              <li key={flashCardSet.id}>
              <Button onClick={() => { navigate(routes.FlashCardSetListing.replace(":id", `${flashCardSet.id}`))}}> 
               {flashCardSet.setName}
              </Button> 
                <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faPencil}
                    onClick={() => {
                      navigate(
                        routes.FlashCardSetUpdate.replace(":id", `${flashCardSet.id}`)
                      );
                    }}
                />
                <Button onClick={() => handleFcSetDelete(flashCardSet.id)} color="red" variant="outline">
                  <FontAwesomeIcon icon={faTrash} />
                </Button>
              <Space h="md" />
            </li>
            ))}
            <Button onClick={() => {navigate(routes.FCSetCreate.replace(":id", `${group.id}`))}}>
            <FontAwesomeIcon icon={faPlus} /> <Space w={8} /> New Set </Button>
          </ul>
          <h1>Assignments</h1>
          <ul>
            {group.assignments.map((assignment) => (
              <li key={assignment.id}>
              <Button onClick={() => { navigate(routes.AssignmentGradeListingg.replace(":id", `${assignment.id}`))}}> 
               {assignment.assignmentName}
              </Button> 
                 <FontAwesomeIcon
                      className={classes.iconButton}
                      icon={faPencil}
                      onClick={() => {
                        navigate(
                          routes.AssignmentUpdate.replace(":id", `${assignment.id}`)
                        );
                      }}
                    />
                     <Button onClick={() => handleAssignmentDelete(assignment.id)} color="red" variant="outline">
                  <FontAwesomeIcon icon={faTrash} />
                </Button>
                <Space h="md" />
            </li>
            ))}
            <Button onClick={() => {navigate(routes.AssignmentCreatee.replace(":id", `${group.id}`))}}>
            <FontAwesomeIcon icon={faPlus} /> <Space w={8} /> New Set </Button>
          </ul>
        </div>
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
