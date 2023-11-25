
import { useNavigate } from "react-router-dom";
import { Container, Flex, Button, createStyles, Title } from "@mantine/core";
import { showNotification } from "@mantine/notifications";
import api from "../../config/axios";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import { routes } from "../../routes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus, faEllipsisV } from "@fortawesome/free-solid-svg-icons";
import { UpdateDeleteButton } from "./three-dots";
import { useUser } from "../../authentication/use-auth";
import { useEffect, useState } from "react";




const Card = ({ group, navigate, handleGroupDelete }) => {
  const { classes } = useStyles();

  const handleCardClick = () => {
    navigate(routes.GroupHome.replace(":id", `${group.id}`));
  };

  return (
    <div className={classes.groupContainer}>
      <div className={classes.groupCard} onClick={handleCardClick}>
        <div className={classes.groupActions}>
          <UpdateDeleteButton
            onUpdate={() => {
              navigate(routes.GroupUpdate.replace(":id", `${group.id}`));
            }}
            onDelete={() => {
              handleGroupDelete(group.id);
            }}
          />
        </div>
        <Title order={2} style={{ margin: 0 }}>
          {group.groupName}
        </Title>
        <p style={{ margin: 0, textAlign: 'center', fontSize: `17px`,fontWeight: 'bold' , padding: `16px 5px 0px`}}>{`Members: ${group.users.length}`}</p>
        <p style={{padding: `0 10px 100px`,textAlign: 'center', fontSize: `15px` }}>{group.description}</p>

      </div>
    </div>
  );
};

const useStyles = createStyles(() => {
  const cardBackgroundColor = '#333'; // Change this to the desired card background color
  const buttonColor = '#3498db'; // Change this to the desired button color

  return {
    groupCard: {
      position: 'relative',
      width: '280px',
      height: '250px',
      boxSizing: 'border-box',
      marginBottom: '20px',
      padding: '16px 5px 0px',
      display: 'flex',
      flexDirection: 'column', 
      alignItems: 'center',
      justifyContent: 'space-between',
      backgroundColor: cardBackgroundColor,
      color: '#fff',
      boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
      borderRadius: '8px',
      transition: 'transform 0.3s, box-shadow 0.3s',
      cursor: 'pointer',

      '&:hover': {
        transform: 'scale(1.05)',
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
      },
    },

    groupContainer: {
      width: 'calc(33.33% - 20px)',
      boxSizing: 'border-box',
      marginBottom: '20px',
      alignItems: `center`,
      marginLeft: 'auto',
  marginRight: 'auto',
    },

    groupActions: {
      position: 'absolute',
      top: '10px',
      right: '10px',
      zIndex: 1,
    },

    // Style for the overlapping button
    '.update-delete-button': {
      position: 'absolute',
      top: 0,
      right: 0,
      zIndex: 2,
      cursor: 'pointer',
      padding: '8px',
      backgroundColor: buttonColor,
      color: '#fff',
    },
  };
});

const GroupListing = () => {
  const [group, setGroup] = useState<GroupGetDto[]>();
  const navigate = useNavigate();
  const { classes } = useStyles();
  const user = useUser();

 
  async function fetchGroup() {
    try {
      const response = await api.get<ApiResponse<GroupGetDto[]>>(`/api/Groups/ByUserId/${user.id}`);
      setGroup(response.data.data);
    } catch (error) {
      console.error("Error fetching groups:", error);
    }
  }

  const handleGroupDelete = async (groupId: number) => {
    try {
      await api.delete(`/api/Groups/${groupId}`);
      showNotification({ message: "Group has been deleted" });
      fetchGroup();
    } catch (error) {
      console.error("Error deleting group:", error);
      showNotification({
        title: "Error",
        message: "Failed to delete the group",
      });
    }
  };

  useEffect(() => {
    fetchGroup();
  }, []);

  return (
    <Container style={{maxHeight: '100vh' }}>
      <Flex direction="column" align="center" style={{ marginBottom: '20px' }}>
        <Title order={2} style={{fontSize: '40px', marginBottom: `15px` }}>Hi {user.firstName}, welcome to SchoolMe!</Title>
        <Title order ={4} style={{marginBottom: `40px`, fontSize: `15px`}}>To get started, select a group you're already in or create your own group.</Title>
        <Title order={2} style={{ marginBottom: '18px', fontSize: `35px` }}>Your Groups</Title>
        <Button
          onClick={() => {
            navigate(routes.groupCreate);
          }}
        >
          <FontAwesomeIcon icon={faPlus}/> &nbsp;&nbsp;Create Group
        </Button>
      </Flex>

      {group && (
        <Flex style={{ flexWrap: 'wrap' }}>
          {group.map((group) => (
            <Card
              key={group.id}
              group={group}
              navigate={navigate}
              handleGroupDelete={handleGroupDelete}
            />
          ))}
        </Flex>
      )}
    </Container>
  );
};

export { GroupListing };
