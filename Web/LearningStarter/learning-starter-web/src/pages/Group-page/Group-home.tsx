import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { Button, Container, Space, Title, createStyles } from "@mantine/core";
import api from "../../config/axios";
import { routes } from "../../routes";

export const GroupHome = () => {
  const { id, groupName } = useParams();
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
    <div>
      {group && (
        <div>
          <h1>Group Name: {group.groupName}</h1>
          <p>Description: {group.description}</p>
          <h2>Tests for Group</h2>
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
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};