
import { useEffect, useState } from "react";
import { GroupGetDto, ApiResponse } from "../../constants/types";
import {  Header, Space, Table } from "@mantine/core";
import { Container } from "@mantine/core";
import api from "../../config/axios";




export const GroupListing =  () => {
    const [group, setGroup] = useState<GroupGetDto[]>();

    useEffect(() => {
        fetchGroup();

        async function fetchGroup() {
            const response = await api.get<ApiResponse<GroupGetDto[]>>("/api/Groups");
           
                setGroup(response.data.data);
            
            
        }
    }); 

    return (
    
        <Container>
        <Header height={32}>Group</Header>
        <Space h="md" />
        <Table withBorder striped>
            <thead>
                <tr>
                    <th>Group Name</th>
                    <th>Description</th>
                </tr>
            </thead>
            <tbody>
                {group?.map((group, index) => {
                    return (
                        <tr key={index}>
                            <td>{group.groupName}</td>
                            <td>{group.description}</td>
                        </tr>
                    );
                })}
            </tbody>
        </Table>
    </Container>
    )
}    
