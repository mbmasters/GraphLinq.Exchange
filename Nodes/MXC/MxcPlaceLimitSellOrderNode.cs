﻿using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NodeBlock.Plugin.Exchange.Nodes.MXC
{
    [NodeDefinition("MxcPlaceLimitSellOrderNode", "Place Limit Sell Order", NodeTypeEnum.Function, "MXC")]
    [NodeGraphDescription("Allows you to place a limit sell order on mxc. example result : true for success order and false for fail")]
    public class MxcPlaceLimitSellOrderNode : Node
    {
        public MxcPlaceLimitSellOrderNode(string id, BlockGraph graph)
              : base(id, graph, typeof(MxcPlaceLimitSellOrderNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(MxcConnectorNode), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("quantity", new NodeParameter(this, "quantity", typeof(string), true));
            this.InParameters.Add("price", new NodeParameter(this, "price", typeof(string), true));

            this.OutParameters.Add("orderId", new NodeParameter(this, "orderId", typeof(string), false));
            this.OutParameters.Add("result", new NodeParameter(this, "result", typeof(bool), false));

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            try
            {
                MxcConnectorNode mxcConnector = this.InParameters["connection"].GetValue() as MxcConnectorNode;


                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("symbol", this.InParameters["symbol"].GetValue().ToString());
                param.Add("price", this.InParameters["price"].GetValue().ToString());
                param.Add("quantity", this.InParameters["quantity"].GetValue().ToString());
                param.Add("trade_type", "ASK");
                param.Add("order_type", "LIMIT_ORDER");

                var result = mxcConnector.Client.Post<dynamic>("/open/api/v2/order/place", param, true); 
                
             

                this.OutParameters["orderId"].SetValue(result.data);
                this.OutParameters["result"].SetValue(true);

            }
            catch (Exception ex)
            {
                this.InParameters["result"].SetValue(false);
                return false;
            }

            return true;
        }
    }
}