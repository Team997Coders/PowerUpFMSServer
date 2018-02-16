import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface BreakdownL2Props {
  alliance: string;
  vault: number;
  park: number;
  autorun: number;
}

export class BreakdownL2 extends React.Component<BreakdownL2Props, {}> {
  constructor(props: BreakdownL2Props) {
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
        VT:  {this.props.vault}
      </div>
      <div className='col-md-2' style={styles}>
        PK:  {this.props.park}
      </div>
      <div className='col-md-2' style={styles}>
        AR: {this.props.autorun}
      </div>
    </div>
  }
}
