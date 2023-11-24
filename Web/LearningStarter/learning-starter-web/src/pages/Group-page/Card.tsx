// Card.jsx
import React from "react";
import { Title } from "@mantine/core";
import { routes } from "../../routes";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { UpdateDeleteButton } from "./three-dots";

const Card = ({ group, navigate, handleGroupDelete }) => {
  const handleCardClick = () => {
    navigate(routes.GroupHome.replace(":id", `${group.id}`));
  };

  return (
    <div className="group-container">
      <div className="group-card" onClick={handleCardClick}>
        <Title order={2} style={{ marginBottom: "8px" }}>
          {group.groupName}
        </Title>
        <p>{group.description}</p>
      </div>
      <div className="group-actions">
        <UpdateDeleteButton onUpdate={() => navigate(routes.GroupUpdate.replace(":id", `${group.id}`))}
          onDelete={() => handleGroupDelete(group.id)} />
      </div>
    </div>
  );
};

export { Card };
