import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Sound } from './Sound';

interface FieldProps {
  message: string
}

export class Field extends React.Component<FieldProps, {}> {

  constructor(props: FieldProps) {
    super(props);
  }

  public render() {
    var styles = {
      color: "yellow",
      fontSize: 240,
      textAlign: "center",
      fontFamily: "'Press Start 2P'"
    }

    return <div style={styles}>
      {this.props.message}
      <Sound url="/audio/ready.mp3" type="audio/mpeg" ref={(sound) => sound != null ? sound.play() : {}}/>
    </div>
  }
}