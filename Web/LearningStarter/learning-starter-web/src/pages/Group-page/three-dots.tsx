import React from 'react';
import { Button, Menu, Text, useMantineTheme } from '@mantine/core';
import { faPencil, faTrash, faEllipsisH } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

interface UpdateDeleteButtonProps {
  onUpdate: () => void;
  onDelete: () => void;
}

export function UpdateDeleteButton({ onUpdate, onDelete }: UpdateDeleteButtonProps) {
    const theme = useMantineTheme();
  
    const handleButtonClick = (event: React.MouseEvent) => {
        event.stopPropagation();
    };

    return (
      <div onClick={handleButtonClick}>
        <Menu position="top-end" width={160} withinPortal>
          <Menu.Target>
            <Button
              rightIcon={<FontAwesomeIcon icon={faEllipsisH} style={{ width: 18, height: 18 }} />}
              pr={12}
              color="gray"
              style={{ backgroundColor: 'transparent', border: 'none' }}
            />
          </Menu.Target>
          <Menu.Dropdown>
            <Menu.Item
              icon={<FontAwesomeIcon icon={faPencil} style={{ width: 16, height: 16 }} />}
              onClick={onUpdate}
            >
              Update
            </Menu.Item>
            <Menu.Item
              icon={<FontAwesomeIcon icon={faTrash} style={{ width: 16, height: 16 }} />}
              onClick={onDelete}
            >
              Delete
            </Menu.Item>
          </Menu.Dropdown>
        </Menu>
      </div>
    );
}
