import { SearchOutlined } from "@ant-design/icons";
import { Button, Input, Space } from "antd";

export const getColumnSearchProps = <T extends object>(
  dataIndex: keyof T, // Use keyof T for better typing of the dataIndex
  setSearchText: React.Dispatch<React.SetStateAction<string>>,
  setSearchedColumn: React.Dispatch<React.SetStateAction<string>>
) => ({
  filterDropdown: ({
    setSelectedKeys,
    selectedKeys,
    confirm,
    clearFilters,
  }: {
    setSelectedKeys: (selectedKeys: React.Key[]) => void;
    selectedKeys: React.Key[];
    confirm: () => void;
    clearFilters: () => void;
  }) => (
    <div style={{ padding: 8 }}>
      <Input
        placeholder={`Search ${dataIndex as string}`} // Explicitly cast dataIndex to string
        value={selectedKeys[0]}
        onChange={(e) =>
          setSelectedKeys(e.target.value ? [e.target.value] : [])
        }
        onPressEnter={() =>
          handleSearch(
            selectedKeys,
            confirm,
            dataIndex as string,
            setSearchText,
            setSearchedColumn
          )
        }
        style={{ marginBottom: 8, display: "block" }}
      />
      <Space>
        <Button
          type="primary"
          onClick={() =>
            handleSearch(
              selectedKeys,
              confirm,
              dataIndex as string,
              setSearchText,
              setSearchedColumn
            )
          }
          icon={<SearchOutlined />}
          size="small"
          style={{ width: 90 }}
        >
          Search
        </Button>
        <Button
          onClick={() => handleReset(clearFilters, confirm, setSearchText)}
          size="small"
          style={{ width: 90 }}
        >
          Reset
        </Button>
      </Space>
    </div>
  ),
  filterIcon: (filtered: boolean) => (
    <SearchOutlined style={{ color: filtered ? "#1890ff" : undefined }} />
  ),
  onFilter: (value: string | number | boolean, record: T) =>
    record[dataIndex]
      ? record[dataIndex]
          .toString()
          .toLowerCase()
          .includes(value.toString().toLowerCase())
      : "",
});

const handleSearch = (
  selectedKeys: React.Key[],
  confirm: () => void,
  dataIndex: string,
  setSearchText: React.Dispatch<React.SetStateAction<string>>,
  setSearchedColumn: React.Dispatch<React.SetStateAction<string>>
) => {
  confirm();
  setSearchText(selectedKeys[0] ? selectedKeys[0].toString() : ""); // Update the search text
  setSearchedColumn(dataIndex); // Set the searched column
};

const handleReset = (
  clearFilters: () => void,
  confirm: () => void,
  setSearchText: React.Dispatch<React.SetStateAction<string>>
) => {
  clearFilters();
  confirm();
  setSearchText(""); // Reset the search text
};
