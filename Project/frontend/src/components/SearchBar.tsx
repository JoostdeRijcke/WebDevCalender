import React from "react";


type Props = {
    value: string;
    onChange: (value: string) => void;
    placeholder?: string;
};


const SearchBar: React.FC<Props> = ({ value, onChange, placeholder = "Search for events" }) => {
    return (
        <input
            type="text"
            value={value}
            onChange={(e) => onChange(e.target.value)}
            placeholder={placeholder}
            style={{
                width: "calc(100% - 32px)",
                margin: 16,
                padding: 10,
                fontSize: 16,
                border: "1px solid #ccc",
                borderRadius: 4,
            }}
            aria-label="Search events"
        />
    );
};


export default SearchBar;