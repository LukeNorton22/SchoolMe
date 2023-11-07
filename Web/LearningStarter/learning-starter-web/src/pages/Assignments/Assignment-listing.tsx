import React, { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import { Button, Center, Container, Flex, Space, Table, Title, createStyles } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate, useParams } from "react-router-dom";
import { ApiResponse, AssignmentGetDto, FlashCardSetGetDto } from "../../constants/types";
import { routes } from "../../routes";
import api from "../../config/axios";
import { faArrowLeft, faPlus, faTruckMonster } from "@fortawesome/free-solid-svg-icons";



export const AssignmentListing = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [assignment, setAssignment] = useState<AssignmentGetDto | null>(null);
  
    useEffect(() => {
      fetchAssignment();
  
      async function fetchAssignment() {
        const response = await api.get<ApiResponse<AssignmentGetDto>>(`/api/Assignments/${id}`);
        if (response.data.hasErrors) {
          
        } else {
          setAssignment(response.data.data);
        }
      }
    }, [id]);
  
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
          Add Question
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
              {assignment.assignmentGrade.map((grade, index) => (
                <tr key={index}>
                  <td>{grade.grade}</td>
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
  