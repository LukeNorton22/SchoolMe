
import { useEffect, useState } from "react";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import {  Button, Flex, Header, Space, Table, Title, createStyles } from "@mantine/core";
import { Container } from "@mantine/core";
import api from "../../config/axios";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTruckMonster, faPlus } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom";
import { routes } from "../../routes";




export const GroupListing =  () => {
    const [group, setGroup] = useState<GroupGetDto[]>();
    const navigate = useNavigate();
    const {classes} = useStyles();

    useEffect(() => {
        fetchGroup();

        async function fetchGroup() {
            const response = await api.get<ApiResponse<GroupGetDto[]>>("/api/Groups");
           
                setGroup(response.data.data);
            
            
        }
    }, []); 

    return (
    
        <Container>
        <Flex direction = "row" justify={"space-between"}>
            <Title order = {3}>Group</Title>
                <Button onClick={() => {
                    navigate(routes.groupCreate);
                }}
                >
                    <FontAwesomeIcon icon = {faPlus}/> <Space w={8} />
                    New Group
                </Button>
        </Flex>
        <Space h="md" />
        {group && (
        <Table withBorder striped>
            <thead>
                <tr>
                    <th></th>
                    <th>Group Name</th>
                    <th>Description</th>
                </tr>
            </thead>
            <tbody>
                {group.map((group, index) => {
                    return (
                        <tr key={index}>
                            <td><FontAwesomeIcon
                             className = {classes.iconButton}
                             icon = {faTruckMonster} 
                             onClick={() =>{
                                navigate(
                                    routes.GroupUpdate.replace(":id", `${group.id}`)
                                );
                            }}/></td>
                            <td>{group.groupName}</td>
                            <td>{group.description}</td>
                        </tr>
                    );
                })}
            </tbody>
        </Table>
        )}
    </Container>
    );
};

const useStyles = createStyles(() => {
    return {
        iconButton: {
            cursor: "pointer",
        },
    };
});