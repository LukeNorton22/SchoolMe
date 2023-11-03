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
    <div>
      {group && (
        <div>
          <h1>Group Name: {group.groupName}</h1>
          <p>Description: {group.description}</p>
          <h2>Tests for Group</h2>
          <ul>
            {group.tests.map((test) => (
              <li key={test.id}>
                <p>Test Name: {test.testName}</p>
                {/* Display other test information here */}
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};
