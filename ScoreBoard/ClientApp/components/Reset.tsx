import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface ResetProps {
  message: string
}

export class Reset extends React.Component<ResetProps, {}> {
  constructor(props: ResetProps) {
    super(props);
  }

  public render() {
    var styles = {
      color: "white",
      fontSize: 160,
      textAlign: "center",
      fontFamily: "'Press Start 2P'"
    }

    return <div style={styles}>
      {this.props.message}
    </div>
  }
}