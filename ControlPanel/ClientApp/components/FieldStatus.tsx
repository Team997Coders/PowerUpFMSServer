import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface FieldStatusState {
  redSwitch: SwitchSettings;
  blueSwitch: SwitchSettings;
  scale: ScaleSettings;
  loading: boolean;
}

interface SwitchSettings {
  alliance: string;
  lhs: string;
  rhs: string;
  ip: string;
}

interface ScaleSettings {
  FLAlliance: string;
  LHSFL: string;
  RHSFL: string;
  IP: string;
}

export class FieldStatus extends React.Component<RouteComponentProps<{}>, FieldStatusState> {
    serverURL: string;
    constructor() {
      super();
      this.state = { 
        redSwitch: { alliance: '', lhs: '', rhs: '', ip: '' }, 
        blueSwitch: { alliance: '', lhs: '', rhs: '', ip: '' }, 
        scale: { FLAlliance: '', LHSFL: '', RHSFL: '', IP: '' },
        loading: true
      };
      this.serverURL = "http://192.168.1.54:5000";

      var red = fetch(`${this.serverURL}/api/switchsettings/red`)
        .then(response => {
          if (response.ok) {
            return response.json() as Promise<SwitchSettings>;
          }
          throw Error("Not found");
        })
        .then(data => {
          this.setState({ redSwitch: data });
        })
        .catch(error => {
          this.setState( { redSwitch: { alliance: 'Not found', lhs: 'N/A', rhs: 'N/A', ip: '0.0.0.0' } } );
        });

      var blue = fetch(`${this.serverURL}/api/switchsettings/blue`)
        .then(response => {
          if (response.ok) {
            return response.json() as Promise<SwitchSettings>;
          }
          throw Error("Not found");
        })
        .then(data => {
          this.setState({ blueSwitch: data });
        })
        .catch(error => {
          this.setState( { blueSwitch: { alliance: 'Not found', lhs: 'N/A', rhs: 'N/A', ip: '0.0.0.0' } } );
        });

      var scale = fetch(`${this.serverURL}/api/scalesettings`)
        .then(response => {
          if (response.ok) {
            return response.json() as Promise<ScaleSettings>;
          }
          throw Error("Not found");
        })
        .then(data => {
          this.setState({ scale: data });
        })
        .catch(error => {
          this.setState( { scale: { FLAlliance: 'Not found', LHSFL: 'N/A', RHSFL: 'N/A', IP: '0.0.0.0' } } );
        });

      Promise.all([red, blue, scale])
        .then(values => this.setState ({loading: false}));
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FieldStatus.renderFieldStatusElements(this.state.redSwitch, this.state.blueSwitch, this.state.scale);

        return <div>
            <h1>Field Status</h1>
            { contents }
        </div>;
    }

    private static renderFieldStatusElements(redSwitch: SwitchSettings, blueSwitch: SwitchSettings, scale: ScaleSettings) {
      return <div> 
        <h2>Scale</h2>
        <div className="control-group">
          <label className="control-label">Alliance Field Left</label>
          <div className="controls readonly">
            {scale.FLAlliance}
          </div>
          <label className="control-label">Left Hand Side Field Left</label>
          <div className="controls readonly">
            {scale.LHSFL}
          </div>
          <label className="control-label">Right Hand Side Field Left</label>
          <div className="controls readonly">
            {scale.RHSFL}
          </div>
          <label className="control-label">IP Address</label>
          <div className="controls readonly">
            {scale.IP}
          </div>
        </div>
        <h2>Red Switch</h2>
        <div className="control-group">
          <label className="control-label">Left Hand Side</label>
          <div className="controls readonly">
            {redSwitch.lhs}
          </div>
          <label className="control-label">Right Hand Side</label>
          <div className="controls readonly">
            {redSwitch.rhs}
          </div>
          <label className="control-label">IP Address</label>
          <div className="controls readonly">
            {redSwitch.ip}
          </div>
        </div>
        <h2>Blue Switch</h2>
        <div className="control-group">
          <label className="control-label">Left Hand Side</label>
          <div className="controls readonly">
            {blueSwitch.lhs}
          </div>
          <label className="control-label">Right Hand Side</label>
          <div className="controls readonly">
            {blueSwitch.rhs}
          </div>
          <label className="control-label">IP Address</label>
          <div className="controls readonly">
            {blueSwitch.ip}
          </div>
        </div>
      </div>;
    }
}
