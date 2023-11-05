import React, { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { Button, Center, Container, Space, Title, createStyles, Navbar } from "@mantine/core";
import api from "../../config/axios";
import { routes } from "../../routes";
import { faArrowLeft, faPlus } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export const GroupHome = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [group, setGroup] = useState<GroupGetDto | null>(null);

  useEffect(() => {
    fetchGroup();

    async function fetchGroup() {
      const response = await api.get<ApiResponse<GroupGetDto>>(`/api/Groups/${id}`);
      setGroup(response.data.data);
    }
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
              <Button onClick={() => {
               navigate(routes.FlashCardSetListing.replace(":id", `${flashCardSet.id}`))
              
               }}
              > 
               {flashCardSet.setName}
              </Button> 
              <Space h="md" />
            </li>
            ))}
          </ul>
        </div>
      )}
    </Container>
  );
}; 