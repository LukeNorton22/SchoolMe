import React, { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import { Button, Center, Container, Flex, Space, Table, Title, createStyles } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate, useParams } from "react-router-dom";
import { ApiResponse, AssignmentGetDto, FlashCardSetGetDto } from "../../constants/types";
import { routes } from "../../routes";
import api from "../../config/axios";

import { faArrowLeft, faPen, faPlus, faTrash, faTruckMonster } from "@fortawesome/free-solid-svg-icons";



export const AssignmentListing = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { classes } = useStyles();
    const [assignment, setAssignment] = useState<AssignmentGetDto | null>(null);
    const [loading, setLoading] = useState(true);
   
    async function fetchAssignment() {
        const response = await api.get<ApiResponse<AssignmentGetDto>>(`/api/assignments/${id}`);
       
          setAssignment(response.data.data);
          setLoading(false);
      }
    
    const handleGradeDelete = async (gradeId: number) => {
      try {
        await api.delete(`/api/assignmentGrade/${id}`);
        showNotification({ message: "grade has entered the trash" });
        fetchAssignment();
      } catch (error) {
        console.error("Error deleting grade:", error);
        showNotification({
          title: "Error",
          message: "Failed to delete the grade",
        });
      }
    };
    useEffect(() => {
      fetchAssignment();
    }, [id]);

    if (loading) {
      return <div>Loading...</div>; // Render a loading indicator
    }
    return (
      <Container>
          <Button
          onClick={() => {
          navigate(routes.GroupHome.replace(":id", `${assignment?.groupId}`));
            }
          }           
              style={{
              backgroundColor: 'transparent',
              border: 'none',
              cursor: 'pointer',
            }}
          >
            <FontAwesomeIcon icon={faArrowLeft} size="xl" /> 
          </Button>
          <Button
          onClick={() => {
            navigate(routes.AssignmentGradeCreate.replace(":id", `${assignment?.id}`));
          }}
        >
          <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
          Add Grade
        </Button>
      <Center>
        <Title >{assignment?.assignmentName}</Title>
        <Space h="lg" />
        </Center>
        {assignment && (
          <Table withBorder fontSize={15}>         
            <thead>
              <tr>
               
                <th>Grades</th>
              </tr>
            </thead>
            <tbody>
              {assignment.grades.map((grade) => (
                <tr >
                  

                  <td>
                  <FontAwesomeIcon
                     className={classes.iconButton}
                    icon={faPen}
                    onClick={() => {
                      navigate(routes.AssignmentGradeUpdate.replace(":id", `${grade.id}`));
                    }}
                    style={{ cursor: 'pointer', marginRight: '8px' }}
                  />
                  <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faTrash}
                    color="red"
                    size="sm"
                    onClick={() => handleGradeDelete(grade.id)}
                    style={{ cursor: 'pointer' }}
                  />
                    {grade.grades}</td>
                </tr>
              ))}
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
  