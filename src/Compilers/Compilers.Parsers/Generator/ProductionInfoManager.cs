﻿// Copyright 2012 Fan Shi
// 
// This file is part of the VBF project.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;

namespace VBF.Compilers.Parsers.Generator
{
    public class ProductionInfoManager
    {
        private IProduction[] m_productions;

        public ProductionInfoManager(IProduction root)
        {
            CodeContract.RequiresArgumentNotNull(root, "root");

            var aggregator = new ProductionAggregationVisitor();
            var productions = root.Accept(aggregator, new List<IProduction>());

            m_productions = productions.ToArray();
            RootProduction = root;

            var ffVisitor = new FirstFollowVisitor();

            bool isChanged;

            do
            {
                isChanged = false;

                foreach (var p in productions)
                {
                    isChanged = p.Accept(ffVisitor, isChanged);
                }

            } while (isChanged);
        }

        public IProduction RootProduction { get; private set; }

        public IReadOnlyList<IProduction> Productions
        {
            get
            {
                return m_productions;
            }
        }

        public ProductionInfo GetInfo(IProduction production)
        {
            return (production as ProductionBase).Info;
        }
    }
}
