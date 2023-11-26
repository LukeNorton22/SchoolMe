// Card.jsx
import React from "react";
import { Title } from "@mantine/core";
import { routes } from "../../routes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { UpdateDeleteButton } from "./three-dots";
import { useUser } from "../../authentication/use-auth";

const Card = ({ group, navigate, handleGroupDelete, currentUser }) => {
  const user = useUser();

  const handleCardClick = () => {
    navigate(routes.GroupHome.replace(":id", `${group.id}`));
  };
  console.log('Creator ID:', group.creatorId);
  console.log('User ID:', user.id);
  return (
    <div className="group-container">
      <div className="group-card" onClick={handleCardClick}>
        <Title order={2} style={{ marginBottom: "8px" }}>
          {group.groupName}
        </Title>
        <p>{group.description}</p>
      </div>
      {user.id === group.creatorId && (
        <div className="group-actions">
          <UpdateDeleteButton
            onUpdate={() => navigate(routes.GroupUpdate.replace(":id", `${group.id}`))}
            onDelete={() => handleGroupDelete(group.id)}
          />
        </div>
      )}
    </div>
  );
};

export { Card };
