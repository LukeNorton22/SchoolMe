import { useEffect, useState } from "react";
import { showNotification } from "@mantine/notifications";
import {  Header, Space, Table } from "@mantine/core";
import { Container } from "@mantine/core";
import api from "../../config/axios";
import { TestsGetDto, ApiResponse } from "../../constants/types";




export const TestListing =  () => {
    const [tests, setTests] = useState<TestsGetDto[]>();

    useEffect(() => {
        fetchTests();

        async function fetchTests() { 
            const response = await api.get<ApiResponse<TestsGetDto[]>>("/api/Tests");

            if (response.data.hasErrors) {
                showNotification({ message: "Error fetching Tests." });
            }

            if (response.data.data) {
                setTests(response.data.data);
            }
        }
    }); 

    return (
        
        <Container>
            <Header height={32}>Tests</Header>
            <Space h="md"/>
            <Table withBorder striped>
                <thead> 
                    <tr>
                        <th>
                            Test Name
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {tests?.map((Tests, index) => {
                        return (
                            <tr key={index}>
                            <td>{Tests.testName}</td>
                        </tr>
                        )
                    })}                    
                </tbody>
            </Table>
        </Container>
        
    );
};


