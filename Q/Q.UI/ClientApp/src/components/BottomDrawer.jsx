import React from "react";

import "./BottomDrawer.scss";

const BottomDrawer = ({children, ...props}) => {
	return (
		<div className="bottom-drawer" {...props}>
			{children}
		</div>
	);
};
export default BottomDrawer;
