import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface TimerProps {
  seconds: number;
}

export class Timer extends React.Component<TimerProps, {}> {
  constructor(props: TimerProps) {
    super(props);
  }

  // Note that the rendering lib does not like fonts with spaces in it...thus the quoting bs
  public render() {
    var styles = {
      fontSize: 220,
      color: 'white',
      lineHeight: '1em',
      fontFamily: "'Press Start 2P'",
    }

    return <div style={styles}>
      {this.props.seconds}
    </div>;
  }
}
