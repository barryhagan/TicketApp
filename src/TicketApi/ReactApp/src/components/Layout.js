import React from "react";
import { Container } from "reactstrap";
import NavMenu from "./NavMenu";
import styled from "styled-components";

export default props => (
  <Layout>
    <NavMenu />
    <Container>
      <AppLayout>{props.children}</AppLayout>
    </Container>
  </Layout>
);

const Layout = styled.div`
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  height: 0;
  z-index: 0;
`;

const AppLayout = styled.div`
  display: flex;
  flex-direction: column;
  height: 100vh;
  position: relative;
  z-index: 0;
`;
