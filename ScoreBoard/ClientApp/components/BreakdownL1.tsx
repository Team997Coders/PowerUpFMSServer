import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface BreakdownL1Props {
  alliance: string;
  scale: number;
  switch: number;
  climb: number;
}

export class BreakdownL1 extends React.Component<BreakdownL1Props, {}> {
  constructor(props: BreakdownL1Props) {
    super(props);
  }

  public render() {
    var styles = {
      color: this.props.alliance,
      fontSize: 48,
      lineHeight: '1.5em',
      fontFamily: "'Press Start 2P'"
    }

    return <div>
      <div className='col-md-2' style={styles}>
        SW: {this.props.switch}
      </div>
      <div className='col-md-2' style={styles}>
        SC: {this.props.scale}
      </div>
      <div className='col-md-2' style={styles}>
        CL: {this.props.climb}
      </div>
    </div>
  }
}
