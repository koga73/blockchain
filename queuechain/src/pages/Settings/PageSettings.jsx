import React from "react";
import {Link} from "react-router-dom";

import "./PageSettings.scss";

function PageSettings() {
	return (
		<section id="settings">
			<h2>Settings</h2>
			<Link to="/" title="Back">
				<span className="hide-text">Back</span>
				<i className="fas fa-times"></i>
			</Link>
		</section>
	);
}
export default PageSettings;
