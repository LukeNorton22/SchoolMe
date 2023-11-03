import { Container, createStyles, Text } from "@mantine/core";

export const GroupHome = () => {
  const { classes } = useStyles();
  return (
    <Container className={classes.homePageContainer}>
      <Text size="lg">Group Home</Text>
    </Container>
  );
};

const useStyles = createStyles(() => {
  return {
    homePageContainer: {
      display: "flex",
      justifyContent: "center",
    },
  };
});
