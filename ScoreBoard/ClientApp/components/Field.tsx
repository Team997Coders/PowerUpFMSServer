import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface FieldProps {
  message: string
}

export class Field extends React.Component<FieldProps, {}> {
  constructor(props: FieldProps) {
    super(props);
  }

  public render() {
    var styles = {
      color: "white",
      fontSize: 240,
      textAlign: "center",
      fontFamily: "'Press Start 2P'"
    }

    return <div style={styles}>
      {this.props.message}
    </div>
  }
}