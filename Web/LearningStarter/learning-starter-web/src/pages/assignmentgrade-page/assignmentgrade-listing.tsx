import { useEffect, useState } from "react";

import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { Container, Header, Space, Table, createStyles } from "@mantine/core";
import { faPencil } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate } from "react-router-dom";
import { routes } from "../../routes";
import { ApiResponse, AssignmentGradeGetDto } from "../../constants/types";

export {};
export const AssignmentGradeListing = () => {
  const [assignmentGrade, setAssignmentGrade] =
    useState<AssignmentGradeGetDto[]>();
  const navigate = useNavigate();
  const { classes } = useStyles();
  useEffect(() => {
    fetchAssignmentGrade();

    async function fetchAssignmentGrade() {
      const response = await api.get<ApiResponse<AssignmentGradeGetDto[]>>(
        "/api/assignmentGrade"
      );

      if (response.data.hasErrors) {
        showNotification({ message: "Error fetching assignment grade." });
      }
      if (response.data.data) {
        setAssignmentGrade(response.data.data);
      }
    }
  }, []);

  return (
    <Container>
      <Header height={32}>Assignment Grades</Header>
      <Space h="md" />
      {assignmentGrade && (
        <Table withBorder striped>
          <thead>
            <tr>
              <th></th>
              <th>CreatorId</th>
              <th>Grade</th>
            </tr>
          </thead>
          <tbody>
            {assignmentGrade.map((assignmentgrade) => {
              return (
                <tr>
                  <td>
                    <FontAwesomeIcon
                      className={classes.iconButton}
                      icon={faPencil}
                      onClick={() => {
                        navigate(
                          routes.AssignmentGradeUpdate.replace(
                            ":id",
                            "${assignmentGrade.id}"
                          )
                        );
                      }}
                    />
                  </td>
                  <td>{assignmentgrade.creatorId}</td>
                  <td>{assignmentgrade.grade}</td>
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
