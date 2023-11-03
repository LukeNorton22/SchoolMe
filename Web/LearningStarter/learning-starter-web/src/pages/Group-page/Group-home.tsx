import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { Container, Title, createStyles } from "@mantine/core";
import api from "../../config/axios";

export const GroupHome = () => {
  const { id, groupName } = useParams();
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
      <Title order={3}></Title>
      {group && (
        <div>
          <h2>Group Information</h2>
          <p>Group Name: {group.groupName}</p>
          <p>Description: {group.description}</p>
          {/* Display other group information here */}
        </div>
      )}
      {/* Add other content for the GroupHome page */}
    </Container>
  );
};
