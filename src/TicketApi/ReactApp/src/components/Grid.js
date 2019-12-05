import styled from "styled-components";

export const DataGridHeader = styled.div`
  background: #f6f8f9;
  border-bottom: 1px solid rgba(18, 60, 83, 0.05);
  border-top: 1px solid rgba(18, 60, 83, 0.05);
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  width: 100%;
  font-size: 0.875rem;
  padding: 12px 32px;
`;

export const DataGridHeaderItem = styled.div`
  display: flex;
  flex-direction: column;
  flex-basis: 100%;
  flex: 1;
  font-size: 0.7rem;
  font-weight: 700;
  line-height: 1;
  text-transform: uppercase;
`;

export const DataGridHeaderItemDouble = styled.div`
  display: flex;
  flex-direction: column;
  flex-basis: 100%;
  flex: 2;
  font-size: 0.7rem;
  font-weight: 700;
  line-height: 1;
  text-transform: uppercase;
`;

export const DataGridHeaderItemTen = styled.div`
  display: flex;
  flex-direction: column;
  flex-basis: 100%;
  flex: 10;
  font-size: 0.7rem;
  font-weight: 700;
  line-height: 1;
  margin-left: 10px;
  text-transform: uppercase;
`;

export const DataGridRow = styled.div`
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  width: 100%;
  font-size: 0.875rem;
  padding: 12px 32px;
  &:hover {
    background: rgba(18, 60, 83, 0.02);
  }
`;

export const DataGridItemDouble = styled.div`
  display: flex;
  flex-direction: column;
  flex-basis: 100%;
  flex: 2;
  font-size: 0.875rem;
  small {
    color: #aaa;
    display: block;
    font-size: 0.75rem;
    margin-top: 3px;
  }
`;

export const DataGridItemTen = styled.div`
  display: flex;
  flex-direction: column;
  flex-basis: 100%;
  flex: 10;
  font-size: 0.875rem;
  margin-left: 10px;
  small {
    color: #aaa;
    display: block;
    font-size: 0.75rem;
    margin-top: 3px;
  }
`;

export const DataGridItem = styled.div`
  display: flex;
  flex-direction: column;
  flex-basis: 100%;
  flex: 1;
  font-size: 0.875rem;
  small {
    color: #aaa;
    display: block;
    font-size: 0.75rem;
    margin-top: 3px;
  }
`;
