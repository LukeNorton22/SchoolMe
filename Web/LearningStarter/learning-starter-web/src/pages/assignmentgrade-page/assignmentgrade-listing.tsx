import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Button, Center, Container, Flex, Space, Table, Title, createStyles } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft, faPlus, faTruckMonster } from "@fortawesome/free-solid-svg-icons";
import { routes } from "../../routes";
import api from "../../config/axios";
import { ApiResponse, AssignmentGetDto } from "../../constants/types";

export const GradePage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [assignment, setAssignment] = useState<AssignmentGetDto | null>(null);

  
  useEffect(() => {
    fetchAssignment();

    async function fetchAssignment() {
      try {
        const response = await api.get<ApiResponse<AssignmentGetDto>>(`/api/assignments/${id}`);
        console.log("API Response:", response);
        if (response.data.hasErrors) {
          // Handle errors here
        } else {
          setAssignment(response.data.data);
        }
      } catch (error) {
        console.error("Error fetching assignment:", error);
      }
    }
  }, [id]);
  console.log("Assignment state:", assignment);

  return (
    <Container>
      <Button
        onClick={() => navigate(routes.GroupHome.replace(":id", `${assignment?.groupId}`))}
        style={{
          backgroundColor: "transparent",
          border: "none",
          cursor: "pointer",
        }}
      >
        <FontAwesomeIcon icon={faArrowLeft} size="xl" />
      </Button>
      <Flex direction="row" justify="space-between">
        <Button
          onClick={() => navigate(routes.AssignmentGradeCreate.replace(":id", `${assignment?.id}`))}
        >
          <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
          Add Grade
        </Button>
      </Flex>

      <Center>
        <Title>{assignment?.assignmentName}</Title>
        <Space h="lg" />
      </Center>

      {assignment && assignment.assignmentGrade ? (
        <Table withBorder fontSize={15}>
          <thead>
            <tr>
              <th></th>
              <th>Grades</th>
            </tr>
          </thead>
          <tbody>
            {assignment.assignmentGrade.map((grade, index) => (
              <tr key={index}>
                <td>
                  <FontAwesomeIcon
                    className={classes.iconButton}
                    icon={faTruckMonster}
                    onClick={() => navigate(routes.AssignmentGradeUpdate.replace(":id", `${assignment.id}`))}
                  />
                </td>
                <td>{grade.grade}</td>
              </tr>
            ))}
          </tbody>
        </Table>
      ) : (
        <p>No grades available</p>
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
