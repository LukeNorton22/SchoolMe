import React, { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import { Button, Card, Center, Container, Flex, Space, Table, Title, createStyles } from "@mantine/core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useNavigate, useParams } from "react-router-dom";
import { ApiResponse, AssignmentGetDto, AssignmentGradeGetDto } from "../../constants/types";
import { routes } from "../../routes";
import api from "../../config/axios";
import "./Card.css";
import { faArrowLeft, faPen, faPlus, faTrash } from "@fortawesome/free-solid-svg-icons";
import { GradeCreate } from "../assignmentgrade-page/grade-create";
import { useUser } from "../../authentication/use-auth";
import { UpdateDeleteButton } from "../Group-page/three-dots";


export const AssignmentListing = () => {
  const { id, gradeId } = useParams();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const [assignment, setAssignment] = useState<AssignmentGetDto | null>(null);
  const [grade, setGrade] = useState<AssignmentGradeGetDto | null>(null);
  const [loading, setLoading] = useState(true);
  const user = useUser();


  async function fetchAssignment() {
    try {
      const response = await api.get<ApiResponse<AssignmentGetDto>>(`/api/assignments/${id}`);
      setAssignment(response.data.data);
      setLoading(false);
    } catch (error) {
      console.error("Error fetching assignment:", error);
      showNotification({
        title: "Error",
        message: "Failed to fetch the assignment",
      });
    }
  }

  async function fetchGrades() {
    try {
      const response = await api.get<ApiResponse<AssignmentGradeGetDto>>(`/api/assignmentGrade/${gradeId}`);
      setGrade(response.data.data);
    } catch (error) {
      console.error("Error fetching grades:", error);
      showNotification({
        title: "Error",
        message: "Failed to fetch grades",
      });
    }
  }

  const calculateAverageGrade = () => {
    if (!assignment || !assignment.grades || assignment.grades.length === 0) {
      return "N/A";
    }

    const totalGrades = assignment.grades.reduce((sum, grade) => sum + Number(grade.grades), 0);
    
    const average = totalGrades / assignment.grades.length;
    return average.toFixed(2);
  };

  const handleGradeDelete = async (gradeId: number) => {
    try {
      
      await api.delete(`/api/assignmentGrade/${gradeId}`);
      showNotification({ message: "Grade has been deleted" });
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
    fetchGrades();
    fetchAssignment();
  }, [id]);
  console.log("User ID:", user.id);
  console.log("Assignment User ID:", assignment?.userId);
  
  if (loading) {
    return <div>Loading...</div>; // Render a loading indicator
  }

  return (
    <Container>
      <Button
        onClick={() => {
          navigate(routes.GroupHome.replace(":id", `${assignment?.groupId}`));
        }}
        style={{
          backgroundColor: 'transparent',
          border: 'none',
          cursor: 'pointer',
        }}
      >
        <FontAwesomeIcon icon={faArrowLeft} size="xl" />
      </Button>
  
      {!userHasPostedGrade() && (
        <Button
        style={{backgroundColor:  `#F9E925`, color: `black`}}
          onClick={() => {
            navigate(routes.AssignmentGradeCreate.replace(":id", `${assignment?.id}`));
          }}
        >
          <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
          Add Grade
        </Button>
      )}
  
      {assignment && (
        <Center>
          <Title>{assignment?.assignmentName}</Title>
          <Space h="lg" />
        </Center>
        
      )}
      <Space h="lg"></Space>

  
      {assignment && (
        <Flex>
<Table withBorder fontSize={15} style={{ width: '600px' }}>
  <thead>
    <tr>
      <th style={{ width: '50%' }}>Grades</th>
      <th style={{ width: '25%' }}>User</th>
      <th style={{ width: '25%' }}></th>
    </tr>
  </thead>
  <tbody>
    {assignment.grades.map((grade) => (
      <tr key={grade.id}>
        <td style={{ width: '25%', verticalAlign: 'middle' }}>
          <div style={{ marginLeft: '8px', marginRight: '8px' }}>
            {grade.grades}
          </div>
        </td>
        <td style={{ width: '25%', verticalAlign: 'middle' }}>
          <div>{grade.userName}</div>
        </td>
        <td style={{ width: '25%', verticalAlign: 'middle', alignItems: 'center' }}>
          {grade.userId === user.id && (
            <UpdateDeleteButton
              onUpdate={() => {
                navigate(routes.AssignmentGradeUpdate.replace(":id", `${grade.id}`));
              }}
              onDelete={() => handleGradeDelete(grade.id)}
            />
          )}
        </td>
      </tr>
    ))}
  </tbody>
</Table>
  
          <Space h="lg" />
  
          <Card
            shadow="sm"
            className="custom-card"
            style={{
              marginLeft: '70px', // Adjust the margin as needed
            }}
          >
            <Card.Section>
              <Center>
                <Title order={4}>Average Grade</Title>
              </Center>
            </Card.Section>
            <Card.Section>
              <Center>
                <Title
                  order={2}
                  style={{
                    color: calculateLetterGradeColor(calculateAverageGrade()),
                  }}
                >
                  {calculateLetterGrade(calculateAverageGrade())}
                </Title>
              </Center>
            </Card.Section>
            <Card.Section>
              <Center>
                <Title order={3}>{calculateAverageGrade()}</Title>
              </Center>
            </Card.Section>
          </Card>
        </Flex>
      )}
    </Container>
  );
  
  
  
  // ...
  

  function userHasPostedGrade() {
    // Check if the user has already posted a grade for the assignment
    return assignment && assignment.grades.some(grade => grade.userId === user.id);
  }
  
};


const calculateLetterGrade = (averageGrade) => {
  if (averageGrade >= 97 && averageGrade <= 100) {
    return "A+";
  } 
  if (averageGrade >= 93 && averageGrade <= 96) {
    return "A";
  }
  if (averageGrade >= 90 && averageGrade <= 92) {
    return "A-";
  }else if (averageGrade >= 87 && averageGrade < 90) {
    return "B+";
  } 
  else if (averageGrade >= 83 && averageGrade < 87) {
    return "B";
  }
  else if (averageGrade >= 80 && averageGrade < 83) {
    return "B-";
  }else if (averageGrade >= 77 && averageGrade < 80) {
    return "C+";
  }
  else if (averageGrade >= 73 && averageGrade < 77) {
    return "C";
  } else if (averageGrade >= 70 && averageGrade < 73) {
    return "C-";
  }  else if (averageGrade >= 67 && averageGrade < 70) {
    return "D+";
  }
  else if (averageGrade >= 65 && averageGrade < 67) {
    return "D";
  } else if (averageGrade < 65) {
    return "F";
  }
};

const calculateLetterGradeColor = (averageGrade) => {
  if (averageGrade >= 90 && averageGrade <= 100) {
    return "green"; // Set your desired color for grade A
  } else if (averageGrade >= 80 && averageGrade < 90) {
    return "lightgreen"; // Set your desired color for grade B
  } else if (averageGrade >= 70 && averageGrade < 80) {
    return "yellow"; // Set your desired color for grade C
  } else if (averageGrade >= 60 && averageGrade < 70) {
    return "orange"; // Set your desired color for grade D
  } else if (averageGrade < 60) {
    return "red"; // Set your desired color for grade F
  }

  return "black"; // Default color
};

const useStyles = createStyles(() => {
  return {
    iconButton: {
      cursor: "pointer",
    },
  };
});
