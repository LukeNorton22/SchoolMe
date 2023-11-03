import { useEffect, useState } from "react";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { Button, Flex, Paper, Space, Title, createStyles } from "@mantine/core";
import { Container } from "@mantine/core";
import api from "../../config/axios";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus, faPen } from "@fortawesome/free-solid-svg-icons";
import { Link, useNavigate } from "react-router-dom";
import { routes } from "../../routes";

export const GroupListing = () => {
  const [group, setGroup] = useState<GroupGetDto[]>();
  const navigate = useNavigate();
  const { classes } = useStyles();

  useEffect(() => {
    fetchGroup();

    async function fetchGroup() {
      const response = await api.get<ApiResponse<GroupGetDto[]>>("/api/Groups");

      setGroup(response.data.data);
    }
  }, []);

  return (
    <Container>
      <Flex direction="row" justify={"space-between"}>
        <Title order={3}>Group</Title>
        <Button
          onClick={() => {
            navigate(routes.groupCreate);
          }}
        >
          <FontAwesomeIcon icon={faPlus} /> <Space w={8} />
          New Group
        </Button>
      </Flex>
      <Space h="md" />
      {group && (
        <Flex>
          {group.map((group) => (
            <Button
              key={group.id}
              fullWidth
              className={classes.groupCard}
              onClick={() => {
                navigate(routes.GroupHome.replace(":id", `${group.id}`));
              }}
            >
              <FontAwesomeIcon
                className={classes.iconButton}
                icon={faPen}
                onClick={() => {
                  navigate(routes.GroupUpdate.replace(":id", `${group.id}`));
                }}
              />
              {group.groupName}
            </Button>
          ))}
        </Flex>
      )}
    </Container>
  );
};

const useStyles = createStyles(() => {
  return {
    iconButton: {
      cursor: "pointer",
    },
    groupCard: {
      width: '200px',
      height: '200px',
      padding: '16px',
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'space-between',
      margin: '10px',
      backgroundColor: 'transparent', // Grey background color
      border: '1px solid #ccc', // Add border for visual separation
      cursor: 'pointer',
      transition: 'background-color 0.3s',
    },
    groupCardHover: {
      backgroundColor: '#ddd', // Change background color on hover
    },
  };
});
